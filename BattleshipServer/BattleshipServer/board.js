$(document).ready(function () {
	var num;
	var theShips = [];
	var tablewidth = 0;
	var tableheight = 0;
	
    function tableCreate(num, position) {
        var body = document.getElementById('board');
        var tbl = document.createElement('table');
        tbl.className = "w3-display-" + position;
        tbl.id = 'table-' + position;
        tbl.style.width = num * 10 + 'px';
        tbl.style.height = num * 10 + 'px';
		tablewidth = num;
		tableheight= num;

        for (var i = 0; i < num; i++) {
            var tr = tbl.insertRow();
            for (var j = 0; j < num; j++) {
                var td = tr.insertCell();
                td.style.width = '10px';
                td.style.height = '10px';
				td.className = 'valid';
                td.id = i + ',' + j;
                td.innerHTML = i + ',' + j;
                td.style.border = '1px solid black';
            }
        }
		
        body.appendChild(tbl);
		
    }
    document.getElementById('build').onclick = function () {
        num = parseInt(document.getElementById('boardSize').value, 10);
		if (num < 2){
			num = 2;
		}
		if (num > 15){
			num = 15;
		}
        tableCreate(num, "middle");
        var div = document.getElementById('bCreation');
        div.style.visibility = "hidden";
        div.style.display = "none";
        var table = document.getElementById('table-middle');
        var cells = table.getElementsByTagName('td');
        for (var i = 0; i < cells.length; i++) {
            var cell = cells[i];
			$(cell).data("weight",0);
            cell.onclick = selected;
        }
    };
	
	document.getElementById('sendoff').onclick = function (){
		var table = document.getElementById('table-middle');
		var coords = table.getElementsByTagName('td');
		for (var i=0; i< coords.length; i++){
			if (coords[i].classList.contains('w3-red')&&(!coords[i].classList.contains('sent'))){
				var posString = coords[i].id.split(",");
				var yPos = parseInt(posString[0]);
				var xPos = parseInt(posString[1]);
				shipScan(xPos,yPos);
			}
		}
		var sendOff = {theShips,"xSize": tablewidth,"ySize": tableheight};
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
	};
	
	
    function selected() {
		var coords = this.id.split(",");
		var intcoords = [parseInt(coords[0]), parseInt(coords[1])];
		if (this.classList.contains('valid') && this.classList.contains('w3-red')){
			var ur = (intcoords[0]-1)+","+ (intcoords[1]+1);
			var ul = (intcoords[0]-1)+","+ (intcoords[1]-1);
			var dr = (intcoords[0]+1)+","+ (intcoords[1]+1);
			var dl = (intcoords[0]+1)+","+ (intcoords[1]-1);
			var urcoords = ur.split(",");
			var ulcoords = ul.split(",");
			var drcoords = dr.split(",");
			var dlcoords = dl.split(",");
			var urintcoords = [parseInt(urcoords[0]), parseInt(urcoords[1])];
			var ulintcoords = [parseInt(ulcoords[0]), parseInt(ulcoords[1])];
			var drintcoords = [parseInt(drcoords[0]), parseInt(drcoords[1])];
			var dlintcoords = [parseInt(dlcoords[0]), parseInt(dlcoords[1])];
			
			this.classList.toggle('w3-red');
			if (($(document.getElementById(ur)).data("weight")) == 1){
				document.getElementById(ur).classList.toggle('w3-blue');
				document.getElementById(ur).classList.toggle('valid');
				($(document.getElementById(ur)).data("weight",$(document.getElementById(ur)).data("weight")-1));
			}
			else if(($(document.getElementById(ur)).data("weight") > 1)){
				($(document.getElementById(ur)).data("weight",$(document.getElementById(ur)).data("weight")-1));
			}
			
			if (($(document.getElementById(ul)).data("weight")) == 1){
				document.getElementById(ul).classList.toggle('w3-blue');
				document.getElementById(ul).classList.toggle('valid');
				($(document.getElementById(ul)).data("weight",$(document.getElementById(ul)).data("weight")-1));
			}
			else if(($(document.getElementById(ul)).data("weight") > 1)){
				($(document.getElementById(ul)).data("weight",$(document.getElementById(ul)).data("weight")-1));
			}
			
			if (($(document.getElementById(dr)).data("weight")) == 1){
				document.getElementById(dr).classList.toggle('w3-blue');
				document.getElementById(dr).classList.toggle('valid');
				($(document.getElementById(dr)).data("weight",$(document.getElementById(dr)).data("weight")-1));
			}
			else if(($(document.getElementById(dr)).data("weight") > 1)){
				($(document.getElementById(dr)).data("weight",$(document.getElementById(dr)).data("weight")-1));
			}
			
			if (($(document.getElementById(dl)).data("weight")) == 1){
				document.getElementById(dl).classList.toggle('w3-blue');
				document.getElementById(dl).classList.toggle('valid');
				($(document.getElementById(dl)).data("weight",$(document.getElementById(dl)).data("weight")-1));
			}
			else if(($(document.getElementById(dl)).data("weight") > 1)){
				($(document.getElementById(dl)).data("weight",$(document.getElementById(dl)).data("weight")-1));
			}
			
		}
		else if (this.classList.contains('valid') && !this.classList.contains('w3-red')){
			
			this.classList.toggle('w3-red');
			var ur = (intcoords[0]-1)+","+ (intcoords[1]+1);
			var ul = (intcoords[0]-1)+","+ (intcoords[1]-1);
			var dr = (intcoords[0]+1)+","+ (intcoords[1]+1);
			var dl = (intcoords[0]+1)+","+ (intcoords[1]-1);
			var urcoords = ur.split(",");
			var ulcoords = ul.split(",");
			var drcoords = dr.split(",");
			var dlcoords = dl.split(",");
			var urintcoords = [parseInt(urcoords[0]), parseInt(urcoords[1])];
			var ulintcoords = [parseInt(ulcoords[0]), parseInt(ulcoords[1])];
			var drintcoords = [parseInt(drcoords[0]), parseInt(drcoords[1])];
			var dlintcoords = [parseInt(dlcoords[0]), parseInt(dlcoords[1])];
			

			if ((urintcoords[0]>=0)&&(urintcoords[1]>=0)
				&&(urintcoords[0]<num)&&(urintcoords[1]<num)){
				($(document.getElementById(ur)).data("weight",$(document.getElementById(ur)).data("weight")+1));	
				if(document.getElementById(ur).classList.contains('valid')){
					document.getElementById(ur).classList.toggle('w3-blue');
					document.getElementById(ur).classList.toggle('valid');
			}
			}
			
			if ((ulintcoords[0]>=0)&&(ulintcoords[1]>=0)
				&&(ulintcoords[0]<num)&&(ulintcoords[1]<num)){
				($(document.getElementById(ul)).data("weight",$(document.getElementById(ul)).data("weight")+1));	
				if(document.getElementById(ul).classList.contains('valid')){
					document.getElementById(ul).classList.toggle('w3-blue');
					document.getElementById(ul).classList.toggle('valid');
			}
			}
			
			if ((drintcoords[0]>=0)&&(drintcoords[1]>=0)
				&&(drintcoords[0]<num)&&(drintcoords[1]<num)){
				($(document.getElementById(dr)).data("weight",$(document.getElementById(dr)).data("weight")+1));	
				if(document.getElementById(dr).classList.contains('valid')){
					document.getElementById(dr).classList.toggle('w3-blue');
					document.getElementById(dr).classList.toggle('valid');
			}
			}
			
			if ((dlintcoords[0]>=0)&&(dlintcoords[1]>=0)
				&&(dlintcoords[0]<num)&&(dlintcoords[1]<num)){
				($(document.getElementById(dl)).data("weight",$(document.getElementById(dl)).data("weight")+1));	
				if(document.getElementById(dl).classList.contains('valid')){
					document.getElementById(dl).classList.toggle('w3-blue');
					document.getElementById(dl).classList.toggle('valid');
			}
			}
			
			
			
		}
		
		//var cell = JSON.stringify(this.id);
		//var xmlhttp = new XMLHttpRequest();
		//xmlhttp.open("POST", "/json-handler");
		//xmlhttp.setRequestHeader("Content-Type", "application/json");
		//xmlhttp.send(cell);
        
    }
	
	
	
	function shipScan(xval, yval){
		var shipLen = 0;
		var isVert = false;
		if (document.getElementById((yval)+","+(xval+1)).classList.contains('w3-red')){
			var xtravel = 0;
			while (document.getElementById(yval+","+(xval+xtravel)).classList.contains('w3-red')){
				document.getElementById(yval+","+(xval+xtravel)).classList.toggle('sent');
				xtravel++;
				if ((xtravel + xval)>= num){
					break;
				}
			}
			shipLen = xtravel;
		}
		else if (document.getElementById((yval + 1)+","+(xval)).classList.contains('w3-red')){
			isVert = true;
			var ytravel = 0;
			while (document.getElementById(yval+ ytravel+","+(xval)).classList.contains('w3-red')){
				document.getElementById(yval+ ytravel+","+(xval)).classList.toggle('sent');
				ytravel++;
				if((ytravel+yval)>= num){
					break;
				}
				
			}
			shipLen = ytravel;
		}
		else{
			shipLen = 1;
		}
		//console.log([shipLen,isVert]);
		var currShip = {
			"x": xval,
			"y": yval,
			"length": shipLen,
			"isVertical":isVert
		}
		theShips.push(currShip);
		
	};


});
