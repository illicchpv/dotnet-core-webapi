console.log("index.js");

const {origin, pathname} = window.location;
const nUrl = origin + pathname;
window.history.replaceState(null, null, nUrl);
