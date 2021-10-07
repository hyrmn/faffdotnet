let copyfiles = require('copyfiles');

const files = [
    './node_modules/htmx.org/dist/htmx.min.js',
    './node_modules/hyperscript.org/dist/_hyperscript.min.js',
    './src/wwwroot/js'
]
copyfiles(files, true, () => console.log('js file copy done'));