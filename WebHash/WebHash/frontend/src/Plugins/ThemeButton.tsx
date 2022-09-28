import React, { useContext } from 'react';
import { ThemeContext } from './ThemeContext';

export function ThemeButton(): JSX.Element {
    const { theme, setTheme } = useContext(ThemeContext);

    function swapTheme() {
        if (theme === 'light') {
            setTheme('dark');
        } else {
            setTheme('light');
        }
    };

    return (
        <button onClick={swapTheme}>
            {theme === 'light' ? 'Change to dark mode' : 'Change to light mode'}
        </button>
    );
}