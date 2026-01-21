use serde::{Deserialize, Serialize};
use chrono::{DateTime, Utc};
use uuid::Uuid;

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct Project {
    pub id: Uuid,
    pub name: String,
    pub created_at: DateTime<Utc>,
}

impl Project {
    pub fn new(name: String) -> Self {
        Self {
            id: Uuid::new_v4(),
            name,
            created_at: Utc::now(),
        }
    }
}

#[derive(Debug, Clone, Serialize, Deserialize, PartialEq, Eq)]
pub struct Session {
    pub id: Uuid,
    pub project_id: Uuid,
    pub start_time: DateTime<Utc>,
    pub end_time: Option<DateTime<Utc>>,
}

impl Session {
    pub fn new(project_id: Uuid) -> Self {
        Self {
            id: Uuid::new_v4(),
            project_id,
            start_time: Utc::now(),
            end_time: None,
        }
    }

    pub fn stop(&mut self) {
        self.end_time = Some(Utc::now());
    }

    pub fn duration_secs(&self) -> i64 {
        let end = self.end_time.unwrap_or_else(Utc::now);
        (end - self.start_time).num_seconds()
    }

    pub fn is_active(&self) -> bool {
        self.end_time.is_none()
    }
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct AppState {
    pub projects: Vec<Project>,
    pub current_project_id: Option<Uuid>,
    pub active_session: Option<Session>,
    pub last_updated: DateTime<Utc>,
}

impl Default for AppState {
    fn default() -> Self {
        Self {
            projects: vec![],
            current_project_id: None,
            active_session: None,
            last_updated: Utc::now(),
        }
    }
}

impl AppState {
    pub fn new() -> Self {
        Self::default()
    }

    pub fn add_project(&mut self, name: String) {
        let project = Project::new(name);
        self.projects.push(project);
        self.projects.sort_by(|a, b| a.name.cmp(&b.name));
        self.last_updated = Utc::now();
    }

    pub fn set_current_project(&mut self, project_id: Uuid) {
        self.current_project_id = Some(project_id);
        self.last_updated = Utc::now();
    }

    pub fn start_session(&mut self) {
        if let Some(project_id) = self.current_project_id {
            if self.active_session.is_none() {
                self.active_session = Some(Session::new(project_id));
                self.last_updated = Utc::now();
            }
        }
    }

    pub fn stop_session(&mut self) -> Option<Session> {
        if let Some(mut session) = self.active_session.take() {
            session.stop();
            self.last_updated = Utc::now();
            return Some(session);
        }
        None
    }

    pub fn current_session_elapsed(&self) -> i64 {
        self.active_session
            .as_ref()
            .map(|s| s.duration_secs())
            .unwrap_or(0)
    }

    pub fn current_project_name(&self) -> Option<String> {
        self.current_project_id.and_then(|id| {
            self.projects.iter().find(|p| p.id == id).map(|p| p.name.clone())
        })
    }
}
