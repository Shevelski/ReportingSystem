//async function initMap() {
//    //const { Map } = await google.maps.importLibrary("maps");
//    let el = document.getElementById("map");
//    const center = { lat: 50.394262352081036, lng: 30.6135351930676 };

//    let map = new google.maps.Map(el, {
//        center: center,
//        zoom: 8,
//    });
//}

async function initMap() {
    let el = document.getElementById("map");
    let map = new google.maps.Map(el, {
        zoom: 8,
        center: { lat: 50.394262352081036, lng: 30.6135351930676 }
    });

    // Try HTML5 geolocation.
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            const center = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };

            map.setCenter(center);

            // Optional: Add a marker at the user's location
            var marker = new google.maps.Marker({
                position: center,
                map: map,
                title: 'Your Location'
            });
        }, function () {
            // If the user denies permission, do nothing, map stays centered at default coordinates
        });
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false, map.getCenter());
    }
}

function handleLocationError(browserHasGeolocation, pos) {
    var infoWindow = new google.maps.InfoWindow();
    infoWindow.setPosition(pos);
    infoWindow.setContent(browserHasGeolocation ?
        'Error: The Geolocation service failed.' :
        'Error: Your browser doesn\'t support geolocation.');
    infoWindow.open(map);
}