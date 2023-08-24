let coordinate = document.getElementById("coordinate");

function getScreenDetails() {
  //Function to get User's Window details
  const width = screen.width;
  const height = screen.height;
  return [width, height];
}

function getURLDetails() {
  //Function to get Network details
  const href = location.href;
  const hostname = location.hostname;
  const pathname = location.pathname;
  const protocol = location.protocol;
  const port = location.port;
  return [href, hostname, pathname, protocol, port];
}

function getBrowserDetails() {
  const isCookieEnabled = navigator.cookieEnabled;
  const RAM = navigator.deviceMemory;
  const language = navigator.language;
  const appVersion = navigator.userAgent;
  const isBrowserOnline = navigator.onLine;

  return [isCookieEnabled, RAM, language, appVersion, isBrowserOnline];
}

// Use function in this way
const [width, height] = getScreenDetails();
console.log(width);
console.log(height);

// Result of other function. Check in browser
const [href, hostname, pathname, protocol, port] = getURLDetails();

const [isCookieEnabled, RAM, language, appVersion, isBrowserOnline] = getBrowserDetails();

// Print the above in console to see the output

// quick output
console.log(getURLDetails());
console.log(getBrowserDetails());

