$('input[name="File"]').change(function () {
	if (this.files && this.files[0]) {
		var reader = new FileReader();
		reader.onload = function (e) {
			selectedImage = e.target.result;
			$('#Logo').attr('src', selectedImage);
		};
		reader.readAsDataURL(this.files[0]);
	}
});
/* selectizesss */
var tagOptions = function (tags) {
	var list = [];
	tags.forEach((e, i) => list.push({ value: e }));
	return list;
}
$('#select-tags').selectize({
	persist: false,
	delimiter: ',',
	maxItems: null,
	valueField: 'value',
	labelField: 'value',
	searchField: ['value'],
	options: tagOptions(tags),
	create: function (input) {
		return {
			value: input,
			text: input
		}
	},
});
var selectTagsHandler = function (value) {
	$('input[name="Tags"]').val($('#select-tags')[0].selectize.getValue());
}
$('#select-tags')[0].selectize.on('item_add', selectTagsHandler);
$('#select-tags')[0].selectize.on('item_remove', selectTagsHandler);

if (selectedTags) {
	var selectedTags = tagOptions(selectedTags.split(','));
	for (var i in selectedTags) {
		$('#select-tags')[0].selectize.addItem(
			$('#select-tags')[0].selectize.search(selectedTags[i].value).items[0].id);
	}
}
$('select[name="Category"]').selectize({
	delimiter: ',',
	persist: false,
	create: function (input) {
		return {
			value: input,
			text: input
		}
	}
});