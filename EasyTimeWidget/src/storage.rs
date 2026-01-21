use std::fs;
use std::path::PathBuf;
use directories::ProjectDirs;
use anyhow::{Context, Result};
use crate::models::AppState;

pub fn get_app_dirs() -> Result<ProjectDirs> {
    ProjectDirs::from("com", "jamers99", "EasyTimeWidget")
        .context("Failed to determine app directories")
}

pub fn get_state_file() -> Result<PathBuf> {
    let dirs = get_app_dirs()?;
    let config_dir = dirs.config_dir();
    fs::create_dir_all(config_dir)?;
    Ok(config_dir.join("projects_state.json"))
}

pub fn get_logs_dir() -> Result<PathBuf> {
    let dirs = get_app_dirs()?;
    let data_dir = dirs.data_dir();
    let logs_dir = data_dir.join("logs");
    fs::create_dir_all(&logs_dir)?;
    Ok(logs_dir)
}

pub fn load_state() -> Result<AppState> {
    let state_file = get_state_file()?;
    if state_file.exists() {
        let content = fs::read_to_string(&state_file)?;
        let state = serde_json::from_str(&content)?;
        Ok(state)
    } else {
        Ok(AppState::new())
    }
}

pub fn save_state(state: &AppState) -> Result<()> {
    let state_file = get_state_file()?;
    let content = serde_json::to_string_pretty(state)?;
    fs::write(state_file, content)?;
    Ok(())
}
