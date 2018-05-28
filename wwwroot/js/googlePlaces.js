var autocomplete;
/* Google Places */
function initAutocomplete() {
	// Create the autocomplete object, restricting the search to geographical
	// location types.
	autocomplete = new google.maps.places.Autocomplete(
		(document.getElementById('autocomplete')),
		{ types: ['geocode'] });

	// When the user selects an address from the dropdown, populate the address
	// fields in the form.
	autocomplete.addListener('place_changed', fillInAddress);
}

function fillInAddress() {
	// Get the place details from the autocomplete object.
	var place = autocomplete.getPlace();

	$('input[name="Latitude"]').val(place.geometry.location.lat().toString().replace('.', ','));
	$('input[name="Longitude"]').val(place.geometry.location.lng().toString().replace('.', ','));

}