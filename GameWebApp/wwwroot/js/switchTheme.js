function changeTheme(theme) {
    localStorage.setItem('theme', theme)
    const style = document.documentElement.style
    
    if (theme === "light") {
        style.setProperty("--bg", "var(--light-bg)");
        style.setProperty("--overlay", "var(--light-overlay-bg)");
        style.setProperty("--contrast", "var(--light-contrast)");
        style.setProperty("--high-contrast", "var(--light-high-contrast)");
    } else if (theme === "dark") {
        style.setProperty("--bg", "var(--dark-bg)");
        style.setProperty("--overlay", "var(--dark-overlay-bg)");
        style.setProperty("--contrast", "var(--dark-contrast)");
        style.setProperty("--high-contrast", "var(--dark-high-contrast)");
    }
}