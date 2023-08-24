function getLocation() {
  if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(showPosition, showError);
  } else {
    coordinate.innerHTML = "Geolocation is not supported by this browser.";
  }
}

function showPosition(position) {
  coordinate.innerHTML =
    "Latitude: " +
    position.coords.latitude +
    "<br>Longitude: " +
    position.coords.longitude;
}

function showError(error) {
  switch (error.code) {
    case error.PERMISSION_DENIED:
      coordinate.innerHTML = "User denied the request for Geolocation.";
      break;
    case error.POSITION_UNAVAILABLE:
      coordinate.innerHTML = "Location information is unavailable.";
      break;
    case error.TIMEOUT:
      coordinate.innerHTML = "The request to get user location timed out.";
      break;
    case error.UNKNOWN_ERROR:
      coordinate.innerHTML = "An unknown error occurred.";
      break;
  }
}


getLocation();