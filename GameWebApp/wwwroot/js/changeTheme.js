const THEME_KEY = 'theme';
const LIGHT_THEME_KEY = 'light';
const DARK_THEME_KEY = 'dark';

function changeTheme(theme) {
    if (theme === true) {
        theme = DARK_THEME_KEY
    } else {
        theme = LIGHT_THEME_KEY
    }

    localStorage.setItem(THEME_KEY, theme)
    const style = document.documentElement.style

    style.setProperty("--bg", `var(--${theme}-bg)`);
    style.setProperty("--overlay", `var(--${theme}-overlay-bg)`);
    style.setProperty("--contrast", `var(--${theme}-contrast)`);
    style.setProperty("--high-contrast", `var(--${theme}-high-contrast)`);
}

function checkTheme(input) {
    const currentTheme = localStorage.getItem(THEME_KEY);
    const checkValue = !(currentTheme === null || currentTheme === LIGHT_THEME_KEY);

    if(checkValue) {
        input.checked = "checked";    
    }

    changeTheme(checkValue);
}

const input = document.getElementById('color_mode')

checkTheme(input)
input.addEventListener("click", (e) => changeTheme(e.target.checked));