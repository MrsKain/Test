///////////////////////////////////////////////点击切换

function changes_click(obj,obj_show,sty,eve){
	var ang = rich(obj);
	var ang_img = rich(obj_show);
	ang_img[0].style.display = 'block';
	ang[0].className = sty;
	for(var i=0;i<ang.length;i++){
		ang[i].index = i;
		ang[i][eve] = function(){
			for(var i=0;i<ang.length;i++){
				ang_img[i].style.display = 'none';
				ang[i].className = '';
			};
			this.className = sty;
			ang_img[this.index].style.display = 'block';
		};
	};
};

///////////////////////////////////////////////
///////////////////////////////////////////////选择题选择
function choice(){
	var aChoice = rich('.explain_type02 .li06');
	/////////////////循环每一个选项
	for(var i=0;i<aChoice.length;i++){	
		choice_one(aChoice[i]);
	};
	////////////////////单个选项
	function choice_one(obj){
		var aGo = obj.children;
		for(var i=0;i<aGo.length;i++){
			aGo[i].onclick = function(){
				for(var i=0;i<aGo.length;i++){
					aGo[i].children[0].style.display = 'block';
					aGo[i].children[1].style.display = 'none';
					aGo[i].children[2].style.color='#666666'; 
				};
				this.children[0].style.display = 'none';
				this.children[1].style.display = 'block';
				this.children[2].style.color='white'; 
			};
		};
	};
};

///////////////////