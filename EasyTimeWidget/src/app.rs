use crate::models::AppState;
use crate::storage::{load_state, save_state};
use crate::export::{export_session, open_logs_folder};

pub struct EasyTimeApp {
    state: AppState,
    new_project_name: String,
    show_add_project: bool,
    error_message: Option<String>,
    last_saved: std::time::Instant,
}

impl EasyTimeApp {
    pub fn new(_cc: &eframe::CreationContext<'_>) -> Self {
        let state = load_state().unwrap_or_default();
        Self {
            state,
            new_project_name: String::new(),
            show_add_project: false,
            error_message: None,
            last_saved: std::time::Instant::now(),
        }
    }

    fn save_if_needed(&mut self) {
        if self.last_saved.elapsed() > std::time::Duration::from_secs(1) {
            if let Err(e) = save_state(&self.state) {
                self.error_message = Some(format!("Failed to save state: {}", e));
            }
            self.last_saved = std::time::Instant::now();
        }
    }

    fn format_duration(secs: i64) -> String {
        let hours = secs / 3600;
        let minutes = (secs % 3600) / 60;
        let seconds = secs % 60;
        format!("{:02}:{:02}:{:02}", hours, minutes, seconds)
    }
}

impl eframe::App for EasyTimeApp {
    fn update(&mut self, ctx: &egui::Context, _frame: &mut eframe::Frame) {
        // Set always on top (Windows)
        if ctx.input(|i| i.viewport().close_requested()) {
            let _ = save_state(&self.state);
            ctx.send_viewport_cmd(egui::ViewportCommand::Close);
        }

        egui::CentralPanel::default().show(ctx, |ui| {
            ui.heading("Easy Time Widget");

            // Error display
            if let Some(ref error) = self.error_message {
                ui.colored_label(egui::Color32::RED, error);
                if ui.button("Dismiss").clicked() {
                    self.error_message = None;
                }
            }

            // Project selection / creation
            ui.horizontal(|ui| {
                ui.label("Project:");

                if self.state.projects.is_empty() {
                    ui.label("(No projects yet)");
                } else {
                    let current_name = self
                        .state
                        .current_project_id
                        .and_then(|id| {
                            self.state.projects.iter().find(|p| p.id == id).map(|p| p.name.clone())
                        })
                        .unwrap_or_else(|| "Select...".to_string());

                    // Collect project IDs and names to avoid borrowing issues
                    let projects: Vec<(uuid::Uuid, String)> = self.state.projects
                        .iter()
                        .map(|p| (p.id, p.name.clone()))
                        .collect();

                    ui.menu_button(format!("▼ {}", current_name), |ui| {
                        for (project_id, project_name) in &projects {
                            if ui.button(project_name).clicked() {
                                self.state.set_current_project(*project_id);
                                self.save_if_needed();
                                ui.close_menu();
                            }
                        }
                    });
                }

                if ui.button("➕ New").clicked() {
                    self.show_add_project = !self.show_add_project;
                }
            });

            // Add project UI
            if self.show_add_project {
                ui.horizontal(|ui| {
                    ui.text_edit_singleline(&mut self.new_project_name);
                    if ui.button("Add").clicked() && !self.new_project_name.trim().is_empty() {
                        self.state.add_project(self.new_project_name.trim().to_string());
                        self.new_project_name.clear();
                        self.show_add_project = false;
                        self.save_if_needed();
                    }
                    if ui.button("Cancel").clicked() {
                        self.show_add_project = false;
                        self.new_project_name.clear();
                    }
                });
            }

            ui.separator();

            // Timer display
            let elapsed = self.state.current_session_elapsed();
            let elapsed_str = Self::format_duration(elapsed);

            ui.horizontal(|ui| {
                ui.vertical(|ui| {
                    ui.label("Time:");
                    ui.heading(elapsed_str);
                });

                ui.vertical(|ui| {
                    ui.label(" ");
                    if self.state.active_session.is_some() {
                        let mut button = egui::Button::new("⏹ STOP");
                        button = button.fill(egui::Color32::from_rgb(200, 0, 0));
                        if ui.add(button).clicked() {
                            if let Some(session) = self.state.stop_session() {
                                let _ = export_session(&self.state, &session);
                            }
                            self.save_if_needed();
                        }
                    } else {
                        let mut button = egui::Button::new("▶ START");
                        button = button.fill(egui::Color32::from_rgb(0, 150, 0));
                        if ui.add(button).clicked() {
                            self.state.start_session();
                            self.save_if_needed();
                        }
                    }
                });
            });

            ui.separator();

            // Open logs folder button
            if ui.button("📁 Open Logs Folder").clicked() {
                if let Err(e) = open_logs_folder() {
                    self.error_message = Some(format!("Failed to open folder: {}", e));
                }
            }
        });

        // Auto-save
        self.save_if_needed();

        // Request repaint for continuous timer update
        if self.state.active_session.is_some() {
            ctx.request_repaint();
        }
    }
}
