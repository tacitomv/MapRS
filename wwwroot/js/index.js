$(document).ready(function () {
	$('.cnpj').mask('99.999.999/9999-99');
	$('.year').mask('9999');
});
//$('.datetimepicker').datetimepicker({
//	format: 'Y',
//	locale: 'pt-BR'
//});

//inicializa o mapa
var map = new L.Map('map', {
	center: new L.LatLng(-30.4824658, -52.4516483),
	zoom: 7,
	maxZoom: 18,
	attributionControl: false,
	zoomControl: false,
	scrollWheelZoom: true
});
//adiciona layer de tiles
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(map);

//cluster group
var markers = L.markerClusterGroup();
map.addLayer(markers);

//attribution
L.control.attribution({ prefix: '<a href="#" data-toggle="modal" data-target="#about">Sobre o mapa</a>' }).addTo(map);

//adiciona controle de zoom
L.control.zoom({ position: 'bottomright' }).addTo(map);


//transforma cada lugar, add id e marker (quando poss�vel)
lugares.forEach(function (element) {
	if (element.Latitude && element.Longitude) {
		var marker = new L.marker([
			element.Latitude,
			element.Longitude
		]);
		marker.bindPopup(
			"<b>Nome:</b> " + element.Name
			+ "<br><b>Endereço:</b> " + element.Address
			+ (element.Description ? "<br><b>Descrição:</b> " + element.Description : "")
			+ (element.Tags ? "<br><b>Tags:</b> " + element.Tags : "")
			+ (element.Website ? '<br><b>Site:</b> <a target="_blank" href="' + element.Website + '">' + element.Website + '</a>' : "")
			+ (element.ContactName ? "<br><b>Contato:</b> " + element.ContactName : "")
			+ (element.Phone ? "<br><b>Telefone:</b> " + element.Phone : "")
			+ (element.Email ? '<br><b>E-mail:</b> <a href="mailto:' + element.Email + '">' + element.Email + '</a>' : ""));
		element.marker = marker;
		markers.addLayer(element.marker);
		markers.addLayer(marker);
	}
}, this);

//cria um index
var lugaresIndex = lugares.reduce(function (acc, doc) {
	acc[doc.Id] = doc
	return acc
}, {});

//cria inst�ncia de busca
var idx = lunr(function () {
	this.field('Name', 10)
	this.field('Description')
	this.field('Address')
	this.field('Tags')
	this.ref('Id')

	lugares.forEach(function (doc) {
		this.add(doc);
	}, this);
});

//realiza a busca
function doSearch(evt, value, secondTime) {
	//if (!secondTime) {
	//	if ((this.value == undefined || this.value == "")) {
	//		location.hash = "";
	//	}
	//	else location.hash = "Buscar=" + encodeURI(this.value);
	//}

	if (this.value != null && this.value.length == 0) {
		init();
	}
	else {
		var r = [];
		if (value) {
			r = idx.search(value);
		}
		else r = idx.search(this.value);

		$('#result').html('');
		if (r.length == 0) {
			if (!secondTime) {
				doSearch(null, this.value + '*', true);
			}
			else {
				$('#result').append(`
                        <li class="text-center">
                            <h4 style="padding: 20px;">Não há resultados para a sua pesquisa!</h4>
                        </li>
                        <li class="text-center">
                            <input type="button" data-toggle="modal" data-target="#createPOI" class="btn btn-primary" value="Adicionar Local"/>
                        </li>`);
			}
		}
		else {
			$('#result').html('');
			r.forEach(function (result) {
				var lugar = lugaresIndex[result.ref];
				addResult(lugar);
			}, this);
		}
	}
}
document.getElementById('search').oninput = doSearch;

$('body').on('click', '.marker-link', function (evt) {
	evt.preventDefault();
	var id = $(this).data('id');
	if (id) {
		if (lugaresIndex[id].marker) {
			markers.zoomToShowLayer(lugaresIndex[id].marker);
			//map.setView(lugaresIndex[id].marker.getLatLng(), 14);
			setTimeout(() => lugaresIndex[id].marker.openPopup(), 100);
		}
	}
});

function addResult(lugar) {
	$('#result').append('<li><a href="#" class="marker-link" data-id="'
		+ lugar.Id + '"><div class="box">'
		+ (lugar.Logo ? '<div class="left"><div><img src="' + lugar.Logo + '"/></div></div><div class="right"><div>' : '<div class="right no-left"><div>')
		+ '<b>' + lugar.Name + '</b><br />'
		+ lugar.Category + ''
		//+ lugar.Address + '<br />'
		//+ lugar.Description + '<br />'
		//+ (lugar.Tags ? lugar.Tags + '<br />' : '')
		//+ (lugar.Website ? lugar.Website + '<br />' : '')
		+ '</div></div></div></a></li>');
}

$('.category-item').on('click', function (evt) {
	var val = $(this).text();
	$('#category-label').text(val);
	if (val == 'Todas') {
		init();
		return;
	}
	$('#result').html('');
	lugares.forEach(function (lugar) {
		if (lugar.Category == val) addResult(lugar);
	}, this);
});

function init() {
	lugares.forEach(function (lugar) {
		addResult(lugar);
	}, this);
}
init();

/* selectizesss */
function transform(lista) {
	var list = [];
	lista.forEach((e, i) => list.push({ value: e }));
	return list;
}
var multipleSelectOptions = {
	persist: false,
	delimiter: ',',
	maxItems: null,
	valueField: 'value',
	labelField: 'text',
	searchField: ['text'],
	dropdownParent: null,
	create: false
}
$('#select-tags').selectize({
	persist: false,
	delimiter: ',',
	maxItems: null,
	valueField: 'value',
	labelField: 'value',
	searchField: ['value'],
	dropdownParent: null,
	create: function (input) {
		return {
			value: input,
			text: input
		}
	},
	render: {
		option_create: function (data, escape) {
			return '<div class="create">Adicionar <strong>' + escape(data.input) + '</strong>&hellip;</div>';
		}
	}
});
$('#select-tags')[0].selectize.addOption(transform(tags));
var selectTagsHandler = function (value) {
	$('input[name="Tags"]').val($('#select-tags')[0].selectize.getValue());
}
$('#select-tags')[0].selectize.on('item_add', selectTagsHandler);
$('#select-tags')[0].selectize.on('item_remove', selectTagsHandler);
$('select[name="Category"]').selectize({
	delimiter: ',',
	persist: false,
	create: function (input) {
		return {
			value: input,
			text: input
		}
	},
	render: {
		option_create: function (data, escape) {
			return '<div class="create">Adicionar <strong>' + escape(data.input) + '</strong>&hellip;</div>';
		}
	},
	onChange: function (val) {
		if (val == 'Startups') {
			$('.secret').removeClass('hidden').addClass('animated fadeIn');
			$('#select-segments')[0].selectize.settings.placeholder = "Segmentos de atuação *";
			$('#select-tecnologies')[0].selectize.settings.placeholder = "Tecnologias *";
			$('#select-segments')[0].selectize.updatePlaceholder();
			$('#select-tecnologies')[0].selectize.updatePlaceholder();
		}
		else if (val == 'Empresas') {
			$('.secret').removeClass('hidden').addClass('animated fadeIn');
			$('#select-segments')[0].selectize.settings.placeholder = "Segmentos de interesse *";
			$('#select-tecnologies')[0].selectize.settings.placeholder = "Tecnologias de interesse *";
			$('#select-segments')[0].selectize.updatePlaceholder();
			$('#select-tecnologies')[0].selectize.updatePlaceholder();
		}
		else
			$('.secret').removeClass('animated fadeIn').addClass('hidden');
	}
});

if (segments) {
	$.each(segments, function () {
		$('#select-segments').append($("<option></option>").val(this.value).html(this.text));
	});
}
if (tecnologies) {
	$.each(tecnologies, function () {
		$('#select-tecnologies').append($("<option></option>").val(this.value).html(this.text));
	});
}
//$('#select-segments').selectize(multipleSelectOptions);
//$('#select-segments')[0].selectize.addOption(segments);
//$('#select-tecnologies').selectize(multipleSelectOptions);
//$('#select-tecnologies')[0].selectize.addOption(tecnologies);

$("#createPOIForm").submit(function (e) {
	$('#createPOIForm > button.btn.btn-primary').prop('disabled', true);
	e.preventDefault();
	var data = new FormData(this);
	var aux = $(this).serializeArray();
	for (var item in aux) {
		if (aux[item].name == "FoundationYear")
			data.append("FoundationDate", aux[item].value + "-01-01");
	}
	// var data = $(this).serializeArray();
	// for(var item in data){
	//     if(data[item].name == "FoundationYear")
	//         data.push({ name: "FoundationDate", value: data[item].value + "-01-01" });
	//     if(data[item].name == "Latitude" || data[item].name == "Longitude")
	//         data[item].value = data[item].value.replace('.',',');
	// }
	$.ajax(this.action, {
		method: this.method,
		data: data,
		cache: false,
		contentType: false,
		processData: false
	}).then((rtrn) => {
		if (rtrn.status == "OK") {
			alert("Obrigado! Seu Ponto de Interesse foi registrado e em breve será adicionado ao mapa.");
		}
		else {
			console.log(rtrn.message);
			alert("Hmm... algo errado não está certo.");
		}
		$('#createPOI').modal('hide');
		$('body').removeClass('modal-open');
		$('.modal-backdrop').remove();
		$('#createPOIForm > button.btn.btn-primary').prop('disabled', false);
	});
});

$('#createPOI').on('shown.bs.modal', () => {
	location.hash = "Cadastrar";
});
$('#createPOI').on('hidden.bs.modal', () => {
	location.hash = "";
});

$(function () {
	if (location.hash) {
		var place = location.hash.split('#')[1];
		if (place == 'Cadastrar') {
			$('#createPOI').modal('show');
		}
		if (place.startsWith('Buscar=')) {
			var query = decodeURI(place.substr(7));
			$('#search').val(query);
			doSearch(null, query);
		}
	}
})