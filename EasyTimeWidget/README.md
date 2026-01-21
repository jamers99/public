# EasyTimeWidget

A simple, always-on-top time tracking widget for Windows desktop. Track time spent on different projects with an intuitive UI and export your data to CSV.

## Features

- **Always-On-Top Window**: Small, persistent widget stays on top of other windows
- **Project Tracking**: Create and select from a list of projects
- **Session Management**: Start/stop timer for current project with one click
- **Elapsed Time Display**: Real-time display of session duration
- **Local Storage**: All data stored locally on your machine (no cloud required)
- **CSV Export**: Export tracked sessions to CSV for analysis
- **Future Notion Integration**: Ready for Notion sync (phase 2)

## Architecture

### Data Model

**Project**
```
- id: UUID
- name: String
- created_at: ISO8601 timestamp
```

**Session**
```
- id: UUID
- project_id: UUID
- start_time: ISO8601 timestamp
- end_time: Option<ISO8601 timestamp> (None if session is running)
- duration_secs: i64 (calculated on stop or export)
```

**AppState**
```
- projects: Vec<Project>
- current_project_id: Option<UUID>
- active_session: Option<Session> (currently running)
- last_updated: ISO8601 timestamp
```

### Storage Strategy

**State File** (`projects_state.json`)
- Location: `%APPDATA%/EasyTimeWidget/`
- Contains: List of projects and current active session (if any)
- Format: JSON
- Persists on every state change (start/stop/project selection)

**Session Log** (`sessions_YYYY-MM-DD.csv`)
- Location: `%APPDATA%/EasyTimeWidget/logs/`
- Format: CSV with headers: `project_name, start_time, end_time, duration_secs, notes`
- One file per day, append-only
- Sessions are logged when stopped
- Exportable directly for analysis or future Notion sync

### UI Layout

- **Window**: 400px × 150px, always on top
- **Project Dropdown**: Select active project (sorted alphabetically)
- **Add Project Button**: Quick input for new project
- **Start/Stop Button**: Toggle session (green "Start" or red "Stop")
- **Elapsed Time Label**: Large, prominent display of current session duration
- **Total Today Label**: Sum of all sessions for selected project today
- **Export Button**: Save all sessions to CSV file and open folder

### Keyboard Shortcuts

- `Space`: Start/Stop current timer
- `Ctrl+E`: Export sessions to CSV
- `Alt+F4`: Close application (graceful shutdown, saves state)

### Dependencies

- `eframe`, `egui`: UI framework
- `serde`, `serde_json`: JSON serialization
- `csv`: CSV writing
- `chrono`: Date/time handling
- `uuid`: Unique identifiers
- `directories`: Cross-platform config paths
- `winit`: Window management (for always-on-top flag)

### Build & Run

**Requirements**
- Rust 1.70+ with MSVC toolchain
- Windows 10+

**Build**
```bash
cargo build --release
```

**Run**
```bash
cargo run --release
# or directly
.\target\release\EasyTimeWidget.exe
```

**Package (Optional - Future)**
- Single `.exe` executable (no installer needed for now)
- Future: MSI installer via `cargo-wix`

### Data Location

All data stored in: `%APPDATA%/EasyTimeWidget/`
- `projects_state.json`: Current state
- `logs/sessions_YYYY-MM-DD.csv`: Daily session logs

### CSV Export Format

```
project_name,start_time,end_time,duration_secs
My Project,2026-01-21T09:00:00Z,2026-01-21T09:30:00Z,1800
My Project,2026-01-21T10:00:00Z,2026-01-21T10:45:00Z,2700
```

### Future Enhancements

1. **Notion Integration**: Sync sessions to Notion database via REST API
2. **Reports**: Weekly/monthly time summaries
3. **Statistics**: Charts and analytics
4. **Themes**: Dark mode
5. **Sync**: Cloud backup (optional, local-first approach)

### Known Limitations

- Windows-only (for now)
- Notion sync not yet implemented
- No database backend (JSON + CSV sufficient for solo use)
