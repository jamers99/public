use std::fs::{self, OpenOptions};
use csv::Writer;
use chrono::Utc;
use anyhow::Result;
use crate::models::{Session, AppState};
use crate::storage::get_logs_dir;

/// Append a completed session to the daily CSV file
pub fn export_session(state: &AppState, session: &Session) -> Result<()> {
    if session.end_time.is_none() {
        return Ok(()); // Don't export incomplete sessions
    }

    let logs_dir = get_logs_dir()?;
    let today = Utc::now().format("%Y-%m-%d").to_string();
    let log_file = logs_dir.join(format!("sessions_{}.csv", today));

    let project_name = state
        .projects
        .iter()
        .find(|p| p.id == session.project_id)
        .map(|p| p.name.clone())
        .unwrap_or_else(|| "Unknown".to_string());

    // Check if file exists to determine if we need to write headers
    let file_exists = log_file.exists();
    let file_is_empty = if file_exists {
        fs::metadata(&log_file)?.len() == 0
    } else {
        true
    };

    // Open in append mode
    let file = OpenOptions::new()
        .create(true)
        .append(true)
        .open(&log_file)?;

    let mut writer = Writer::from_writer(file);

    // Only write header if file is empty
    if file_is_empty {
        writer.write_record(&["project_name", "start_time", "end_time", "duration_secs"])?;
    }

    let duration_secs = session.duration_secs();
    let start_time = session.start_time.to_rfc3339();
    let end_time = session
        .end_time
        .map(|e| e.to_rfc3339())
        .unwrap_or_default();

    writer.write_record(&[
        &project_name,
        &start_time,
        &end_time,
        &duration_secs.to_string(),
    ])?;

    // Explicitly flush the writer to ensure data is written
    drop(writer);
    Ok(())
}

pub fn open_logs_folder() -> Result<()> {
    let logs_dir = get_logs_dir()?;
    #[cfg(target_os = "windows")]
    {
        std::process::Command::new("explorer")
            .arg(logs_dir)
            .spawn()?;
    }
    #[cfg(not(target_os = "windows"))]
    {
        println!("Logs directory: {}", logs_dir.display());
    }
    Ok(())
}
