mod models;
mod storage;
mod export;
mod app;

fn main() -> Result<(), eframe::Error> {
    let options = eframe::NativeOptions {
        viewport: egui::ViewportBuilder::default()
            .with_inner_size([400.0, 200.0])
            .with_position([100.0, 100.0])
            .with_always_on_top(),
        ..Default::default()
    };

    eframe::run_native(
        "EasyTimeWidget",
        options,
        Box::new(|cc| {
            Box::new(app::EasyTimeApp::new(cc))
        }),
    )
}
