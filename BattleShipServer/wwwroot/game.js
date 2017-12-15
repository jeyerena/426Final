$(document).ready(function () {
    var num;
    var playerShip = 'w3-blue';
    var enemyShip = 'w3-red'; //change only when hit
    var missedShot = 'w3-yellow';

    var theShips = [];
    var tablewidth = 0;
    var tableheight = 0;

	
	//var theSocket = new WebSocket("http://testserver4-ztong.cloudapps.unc.edu/");
	
    var initdiv = document.getElementById('bCreation');
    initdiv.style.visibility = "hidden";
    initdiv.style.display = "none";

    var confbutton = document.getElementById('sendoff');
    confbutton.style.visibility = "hidden";
    confbutton.style.display = "none";

	

    document.getElementById('unhidem').onclick = function () {
        var div = document.getElementById('lCreation');
        div.style.visibility = "hidden";
        div.style.display = "none";
        initdiv.style.visibility = "visible";
        initdiv.style.display = "block";
    };

    document.getElementById('build').onclick = function () {
		document.getElementById("cimage").style.marginTop = "-70px";
        num = parseInt(document.getElementById('boardSize').value, 10);
        tableCreate(num, "top");
        tableCreate(num, "bottom");

        var div = document.getElementById('lCreation');
        div.style.visibility = "hidden";
        div.style.display = "none";
        var table = document.getElementById('table-top');
        var cells = table.getElementsByTagName('td');
        for (var i = 0; i < cells.length; i++) {
            var cell = cells[i];
            cell.onclick = shot;
        }
        //have it receive the normal player data from the server
        //or pullit from previous 'page'
        var tmp = [];
        tmp.push({ 'x': 0, 'y': 0, 'length': 2, 'isVertical': false });
        tmp.push({ 'x': 3, 'y': 0, 'length': 3, 'isVertical': true });
        tmp.push({ 'x': 6, 'y': 0, 'length': 4, 'isVertical': false });
        tmp.push({ 'x': 0, 'y': 2, 'length': 4, 'isVertical': true });
        tmp.push({ 'x': 5, 'y': 2, 'length': 3, 'isVertical': false });
        tmp.push({ 'x': 9, 'y': 2, 'length': 2, 'isVertical': true });
        tmp.push({ 'x': 0, 'y': 7, 'length': 2, 'isVertical': false });
        tmp.push({ 'x': 5, 'y': 5, 'length': 2, 'isVertical': true });
        tmp.push({ 'x': 9, 'y': 5, 'length': 3, 'isVertical': true });
        tmp.push({ 'x': 5, 'y': 9, 'length': 6, 'isVertical': false });
        placeShips('bottom', JSON.stringify(tmp));
        // placeShips('bottom', JSON.stringify({'x':0, 'y':0, 'length':2, 'isVertical':true}));		
    };

    function tableCreate(num, position) {
        var body = document.getElementById('board');
        var tbl = document.createElement('table');
        tbl.id = 'table-' + position;
        tbl.style.margin = 20 + 'px';
        tbl.style.tableLayout = 'fixed';
        for (var i = 0; i < num; i++) {
            var tr = tbl.insertRow();
            for (var j = 0; j < num; j++) {
                var td = tr.insertCell();
                td.style.width = (180 / num + num - 1) + 'px';
                td.style.height = (180 / num + num - 1) + 'px';
                td.id = position + '-' + i + ',' + j;
				td.className = 'hasImage';
                // td.innerHTML = i + ',' + j;
                td.style.border = '1px solid black';
            }
        }
		
		
		//if(position == 'bottom'){
		//	 loseon();
		//}
        body.appendChild(tbl);
		//$("#titlestash").prependTo("#board");
    }

    function placeShips(position, shipJSON) {
        var ships = JSON.parse(shipJSON);
        if (!Array.isArray(ships)) {
            ships = [ships];
        }
		console.log(ships);
        for (var i = 0; i < ships.length; i++) {
            var ship = ships[i];
            if (ship.isVertical) {
                if (ship.x >= 0 && ship.x < num && ship.y >= 0) {
                    for (var j = 0; j < ship.length; j++) {
                        if (ship.y + j < num) {
                            var cellID = position + '-' + (ship.y + j) + ',' + ship.x;
                            var cell = document.getElementById(cellID);
                            cell.classList.toggle('hasShip');
                            if (!cell.classList.contains(playerShip)) {
								console.log(ship.length);
								if(j==0){
									cell.classList.toggle('hasShipEndT')
								}
								else if(j==(ship.length-1)){
									cell.classList.toggle('hasShipEndB');
								}
								else{
									cell.classList.toggle('hasShipMiddle2')
								}
								//cell.classList.toggle('hasShipSingle');
                                //cell.classList.toggle(playerShip);
                            }
                        }
                    }
                }
            }
            else {
                if (ship.y >= 0 && ship.y < num && ship.x >= 0) {
                    for (var j = 0; j < ship.length; j++) {
                        if (ship.x + j < num) {
                            var cellID = position + '-' + ship.y + ',' + (ship.x + j);
                            var cell = document.getElementById(cellID);
                            cell.classList.toggle('hasShip');
                            if (!cell.classList.contains(playerShip)) {
								if(ship.length==1){
									cell.classList.toggle('hasShipSingle');
								}
								else if(j==0){
									cell.classList.toggle('hasShipEndL');
								}
								else if(j==(ship.length-1)){
									cell.classList.toggle('hasShipEndR');
								}
								else{
									cell.classList.toggle('hasShipMiddle')
								}
								//cell.classList.toggle('hasImage');
                                //cell.classList.toggle(playerShip);
                            }
                        }
                    }
                }
            }
        }
    }
	
    function radar(cellID) {
        var coordinates = cellID.split('-', 1)[1];
        var cell = {};
        cell['x'] = coordinates.split(',')[0];
        cell['y'] = coordinates.split(',')[1];
        var cell = JSON.stringify(cell);
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.open("POST", "/json-handler");
        xmlhttp.setRequestHeader("Content-Type", "application/json");
        xmlhttp.send(cell);

        var response = $.getJSON();
        response = JSON.stringify(response);
        switch (response) {
            case "hit":
                document.getElementById(cellID).classList.toggle(enemyship);
                break;
            case "miss":
                document.getElementById(cellID).classList.toggle(missedShot);
                //give waiting message
                break;
            case "win":
                //display you-win message
                break;
            case "lose":
                //display you lose message
                break;
			case "missgoagain":
				
            default:
                ;
        }
         //display message based on server response
         //var message = document.createElement('p');
         //message.innerHTML = 'Hit!';
         //document.getElementById('events').appendChild(message);
         //    this.className = '';
    }

    function shot() {
		var message = document.createElement('p');
         message.innerHTML = 'Miss!';
		 message.classList.toggle('fifth-text');
         document.getElementById('events').appendChild(message);
              this.className = '';
        if (this.classList.contains(enemyship) || this.classList.contains(missedShot)) {
            return;
        }
        //remove me
        this.classList.toggle(enemyShip);
        radar(this.id);
    }
	
	function winon () {
    document.getElementById("woverlay").style.display = "block";
	
	};
	
	function loseon () {
    document.getElementById("loverlay").style.display = "block";
	
	};

	document.getElementById('woverlay').onclick = function () {
    document.getElementById("woverlay").style.display = "none";
	};
	
	document.getElementById('loverlay').onclick = function () {
    document.getElementById("loverlay").style.display = "none";
	};
	
	
	
    ///BUILD FUNCTIONS///

    //Included  copy of tableCreate with ids that work with my parsing scheme.
    function tableBuild(num, position) {
        var body = document.getElementById('board');
        var tbl = document.createElement('table');
        tbl.id = 'table-' + position;
        tbl.style.margin = 20 + 'px';
        tbl.style.tableLayout = 'fixed';
        tablewidth = num;
        tableheight = num;
        confbutton.style.visibility = "visible";
        confbutton.style.display = "block";

        for (var i = 0; i < num; i++) {
            var tr = tbl.insertRow();
            for (var j = 0; j < num; j++) {
                var td = tr.insertCell();
                td.style.width = (300 / num + num - 1) + 'px';
                td.style.height = (300 / num + num - 1) + 'px';
                td.className = 'valid hasImage';
                td.id = i + ',' + j;
                // td.innerHTML = i + ',' + j;
                td.style.border = '1px solid black';
            }
        }
        body.appendChild(tbl);
		//$("#titlestash").prependTo("#board");

    }

    document.getElementById('manual').onclick = function () {
		document.getElementById("cimage").style.marginTop = "-260px";
        num = parseInt(document.getElementById('boardSize').value, 10);
        if (num < 2) {
            num = 2;
        }
        if (num > 15) {
            num = 15;
        }
        tableBuild(num, "middle");
        var div = document.getElementById('bCreation');
        div.style.visibility = "hidden";
        div.style.display = "none";
		$('#sendoff').toggleClass('bton');
        var table = document.getElementById('table-middle');
        var cells = table.getElementsByTagName('td');
        for (var i = 0; i < cells.length; i++) {
            var cell = cells[i];
            $(cell).data("weight", 0);
            cell.onclick = selected;
        }
    };

    document.getElementById('sendoff').onclick = function () {
        var table = document.getElementById('table-middle');
        var coords = table.getElementsByTagName('td');
        for (var i = 0; i < coords.length; i++) {
            if (coords[i].classList.contains('w3-red') && (!coords[i].classList.contains('sent'))) {
                var posString = coords[i].id.split(",");
                var yPos = parseInt(posString[0]);
                var xPos = parseInt(posString[1]);
                shipScan(xPos, yPos);
            }
        }
        var sendOff = { theShips, "xSize": tablewidth, "ySize": tableheight };
        $("#sendoff").remove();
        var tab = document.getElementById('table-middle');
        var tiles = tab.getElementsByTagName('td');
        for (var i = 0; i < tiles.length; i++) {
            var tile = tiles[i];
            tile.onclick = "";
        }
        console.log(theShips);
        console.log(sendOff);

        var txt = JSON.stringify(sendOff);
        console.log(txt);
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.open("POST", "/json-handler");
        xmlhttp.send(txt);

        //Temporary preview
        var div = document.getElementById('table-middle');
        div.style.visibility = "hidden";
        div.style.display = "none";
		document.getElementById("cimage").style.marginTop = "-70px";
        tableCreate(num, "top");
        tableCreate(num, "bottom");
		var table = document.getElementById('table-top');
        var cells = table.getElementsByTagName('td');
        for (var i = 0; i < cells.length; i++) {
            var cell = cells[i];
            cell.onclick = shot;
        }
        placeShips('bottom', JSON.stringify(theShips));
    };

    function selected() {
		
        var coords = this.id.split(",");
        var intcoords = [parseInt(coords[0]), parseInt(coords[1])];
        if (this.classList.contains('valid') && this.classList.contains('w3-red')) {
            var ur = (intcoords[0] - 1) + "," + (intcoords[1] + 1);
            var ul = (intcoords[0] - 1) + "," + (intcoords[1] - 1);
            var dr = (intcoords[0] + 1) + "," + (intcoords[1] + 1);
            var dl = (intcoords[0] + 1) + "," + (intcoords[1] - 1);
            var urcoords = ur.split(",");
            var ulcoords = ul.split(",");
            var drcoords = dr.split(",");
            var dlcoords = dl.split(",");
            var urintcoords = [parseInt(urcoords[0]), parseInt(urcoords[1])];
            var ulintcoords = [parseInt(ulcoords[0]), parseInt(ulcoords[1])];
            var drintcoords = [parseInt(drcoords[0]), parseInt(drcoords[1])];
            var dlintcoords = [parseInt(dlcoords[0]), parseInt(dlcoords[1])];

            this.classList.toggle('w3-red');
			this.classList.toggle('hasImage');
            if (($(document.getElementById(ur)).data("weight")) == 1) {
                document.getElementById(ur).classList.toggle('w3-grey');
                document.getElementById(ur).classList.toggle('valid');
                ($(document.getElementById(ur)).data("weight", $(document.getElementById(ur)).data("weight") - 1));
            }
            else if (($(document.getElementById(ur)).data("weight") > 1)) {
                ($(document.getElementById(ur)).data("weight", $(document.getElementById(ur)).data("weight") - 1));
            }

            if (($(document.getElementById(ul)).data("weight")) == 1) {
                document.getElementById(ul).classList.toggle('w3-grey');
                document.getElementById(ul).classList.toggle('valid');
                ($(document.getElementById(ul)).data("weight", $(document.getElementById(ul)).data("weight") - 1));
            }
            else if (($(document.getElementById(ul)).data("weight") > 1)) {
                ($(document.getElementById(ul)).data("weight", $(document.getElementById(ul)).data("weight") - 1));
            }

            if (($(document.getElementById(dr)).data("weight")) == 1) {
                document.getElementById(dr).classList.toggle('w3-grey');
                document.getElementById(dr).classList.toggle('valid');
                ($(document.getElementById(dr)).data("weight", $(document.getElementById(dr)).data("weight") - 1));
            }
            else if (($(document.getElementById(dr)).data("weight") > 1)) {
                ($(document.getElementById(dr)).data("weight", $(document.getElementById(dr)).data("weight") - 1));
            }

            if (($(document.getElementById(dl)).data("weight")) == 1) {
                document.getElementById(dl).classList.toggle('w3-grey');
                document.getElementById(dl).classList.toggle('valid');
                ($(document.getElementById(dl)).data("weight", $(document.getElementById(dl)).data("weight") - 1));
            }
            else if (($(document.getElementById(dl)).data("weight") > 1)) {
                ($(document.getElementById(dl)).data("weight", $(document.getElementById(dl)).data("weight") - 1));
            }

        }
        else if (this.classList.contains('valid') && !this.classList.contains('w3-red')) {

            this.classList.toggle('w3-red');
			this.classList.toggle('hasImage');
            var ur = (intcoords[0] - 1) + "," + (intcoords[1] + 1);
            var ul = (intcoords[0] - 1) + "," + (intcoords[1] - 1);
            var dr = (intcoords[0] + 1) + "," + (intcoords[1] + 1);
            var dl = (intcoords[0] + 1) + "," + (intcoords[1] - 1);
            var urcoords = ur.split(",");
            var ulcoords = ul.split(",");
            var drcoords = dr.split(",");
            var dlcoords = dl.split(",");
            var urintcoords = [parseInt(urcoords[0]), parseInt(urcoords[1])];
            var ulintcoords = [parseInt(ulcoords[0]), parseInt(ulcoords[1])];
            var drintcoords = [parseInt(drcoords[0]), parseInt(drcoords[1])];
            var dlintcoords = [parseInt(dlcoords[0]), parseInt(dlcoords[1])];


            if ((urintcoords[0] >= 0) && (urintcoords[1] >= 0)
                && (urintcoords[0] < num) && (urintcoords[1] < num)) {
                ($(document.getElementById(ur)).data("weight", $(document.getElementById(ur)).data("weight") + 1));
                if (document.getElementById(ur).classList.contains('valid')) {
                    document.getElementById(ur).classList.toggle('w3-grey');
                    document.getElementById(ur).classList.toggle('valid');
                }
            }

            if ((ulintcoords[0] >= 0) && (ulintcoords[1] >= 0)
                && (ulintcoords[0] < num) && (ulintcoords[1] < num)) {
                ($(document.getElementById(ul)).data("weight", $(document.getElementById(ul)).data("weight") + 1));
                if (document.getElementById(ul).classList.contains('valid')) {
                    document.getElementById(ul).classList.toggle('w3-grey');
                    document.getElementById(ul).classList.toggle('valid');
                }
            }

            if ((drintcoords[0] >= 0) && (drintcoords[1] >= 0)
                && (drintcoords[0] < num) && (drintcoords[1] < num)) {
                ($(document.getElementById(dr)).data("weight", $(document.getElementById(dr)).data("weight") + 1));
                if (document.getElementById(dr).classList.contains('valid')) {
                    document.getElementById(dr).classList.toggle('w3-grey');
                    document.getElementById(dr).classList.toggle('valid');
                }
            }

            if ((dlintcoords[0] >= 0) && (dlintcoords[1] >= 0)
                && (dlintcoords[0] < num) && (dlintcoords[1] < num)) {
                ($(document.getElementById(dl)).data("weight", $(document.getElementById(dl)).data("weight") + 1));
                if (document.getElementById(dl).classList.contains('valid')) {
                    document.getElementById(dl).classList.toggle('w3-grey');
                    document.getElementById(dl).classList.toggle('valid');
                }
            }



        }


    }

    function shipScan(xval, yval) {
        var shipLen = 0;
        var isVert = false;

        if (xval == (num - 1)) {
            isVert = true;
            var ytravel = 0;
            while (document.getElementById(yval + ytravel + "," + (xval)).classList.contains('w3-red')) {
                document.getElementById(yval + ytravel + "," + (xval)).classList.toggle('sent');
                ytravel++;
                if ((ytravel + yval) >= num) {
                    break;
                }

            }
            shipLen = ytravel;
        }

        else if (yval == (num - 1)) {
            var xtravel = 0;
            while (document.getElementById(yval + "," + (xval + xtravel)).classList.contains('w3-red')) {
                document.getElementById(yval + "," + (xval + xtravel)).classList.toggle('sent');
                xtravel++;
                if ((xtravel + xval) >= num) {
                    break;
                }
            }
            shipLen = xtravel;
        }

        else if (document.getElementById((yval) + "," + (xval + 1)).classList.contains('w3-red')) {
            var xtravel = 0;
            while (document.getElementById(yval + "," + (xval + xtravel)).classList.contains('w3-red')) {
                document.getElementById(yval + "," + (xval + xtravel)).classList.toggle('sent');
                xtravel++;
                if ((xtravel + xval) >= num) {
                    break;
                }
            }
            shipLen = xtravel;
        }
        else if (document.getElementById((yval + 1) + "," + (xval)).classList.contains('w3-red')) {
            isVert = true;
            var ytravel = 0;
            while (document.getElementById(yval + ytravel + "," + (xval)).classList.contains('w3-red')) {
                document.getElementById(yval + ytravel + "," + (xval)).classList.toggle('sent');
                ytravel++;
                if ((ytravel + yval) >= num) {
                    break;
                }

            }
            shipLen = ytravel;
        }
        else {
            shipLen = 1;
        }
        //console.log([shipLen,isVert]);
        var currShip = {
            "x": xval,
            "y": yval,
            "length": shipLen,
            "isVertical": isVert
        }
        theShips.push(currShip);

    };

});
