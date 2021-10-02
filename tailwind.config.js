const defaultTheme = require('tailwindcss/defaultTheme')
let colors = require('tailwindcss/colors')
delete colors['lightBlue'] // <-----
colors = { ...colors, ...{ transparent: 'transparent' } }

colors.transparent = 'transparent';
colors.current = 'currentColor';

module.exports = {
    purge: [
        './src/Pages/**/*.cshtml'
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
