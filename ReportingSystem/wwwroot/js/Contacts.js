async function initMap() {
    //const { Map } = await google.maps.importLibrary("maps");
    let el = document.getElementById("map");
    const center = { lat: 50.394262352081036, lng: 30.6135351930676 };
    let map = new google.maps.Map(el, {
        center: center,
        zoom: 8,
    });
}
