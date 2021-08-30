﻿const defaultTheme = require('tailwindcss/defaultTheme')
const colors = require('tailwindcss/colors')
colors.transparent = 'transparent';
colors.current = 'currentColor';

module.exports = {
    purge: [
        './src/Faff/Pages/**/*.cshtml'
    ],
    darkMode: 'media', // or 'media' or 'class'
    theme: {
        colors,
        extend: {
            fontFamily: {
                sans: ['Inter var', ...defaultTheme.fontFamily.sans],
            },
        },
    },
    variants: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms'),
    ],
}
