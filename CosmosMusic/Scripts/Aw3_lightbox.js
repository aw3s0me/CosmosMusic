(function($){
	$.prototype.Aw3_lightbox = function(properties) {
		var DEFAULT_HEIGHT = "500";
		var DEFAULT_WIDTH = "500";

		var options = $.extend({
			title : null,
			content : null,
			height : DEFAULT_HEIGHT,
			width : DEFAULT_WIDTH,
			top : "20%",
			left : "30%",
		}, properties);

		return this.click(function(event) {
			add_overlay();
			add_box();
			//add_content();
			add_styles();

			$(".Aw3_lightbox").fadeIn();
		});

		function add_styles() {
			$(".Aw3_lightbox").css({
				'position': 'absolute',
				'left' : options.left,
				'top' : options.top,
				'display' : 'none',
				'height': options.height + 'px',
				'width': options.width + 'px',
				'border':'1px solid #fff',
				'box-shadow': '0px 2px 7px #292929',
				'-moz-box-shadow': '0px 2px 7px #292929',
				'-webkit-box-shadow': '0px 2px 7px #292929',
				'border-radius':'10px',
				'-moz-border-radius':'10px',
				'-webkit-border-radius':'10px',
				'background': '#f2f2f2', 
				'z-index':'50',
			});

			$(".Aw3_close").css({
				'position':'relative',
				'top':'-25px',
				'left':'20px',
				'float':'right',
				'display':'block',
				'height':'50px',
				'width':'50px',
				'background': 'url(close.png) no-repeat',
			});

			var pageHeight = $(document).height();
			var pageWidth = $(window).width();

			$('.Aw3_overlay').css({
				'position':'absolute',
				'top':'0',
				'left':'0',
				'background-color':'rgba(0,0,0,0.6)',
				'height':pageHeight,
				'width':pageWidth,
				'z-index':'10'
			});
			
			$('.Aw3_innerbox').css({
				'background-color' : '#fff',
				'height' : (options.height - 50) + 'px',
				'width' : (options.width - 50) + 'px',
				'padding' : '10px',
				'margin':'10px',
				'border-radius':'10px',
				'-moz-border-radius':'10px',
				'-webkit-border-radius':'10px'
			});

			$('.Aw3_innerbox img').css({
				'box-shadow' : '0 0 25px #111',
    			'-webkit-box-shadow' : '0 0 25px #111',
    			'-moz-box-shadow' : '0 0 25px #111',
    			'max-width' : (options.width - 50) + 'px',
    			'max-height' : (options.height - 50) + 'px',
    			'width' : "100%",
    			//'height' : "100%",
    			'height' : '100%',
    			'margin' : '-30px 1px -1px 2px',
    			//'padding' : '2px',
			});
		}

		function add_overlay() {
			var Aw3_overlay = "<div class='Aw3_overlay'></div>";

			$(Aw3_overlay).appendTo("body");
		}

		//function add_content() {}

		function add_box() {
			if (options.title === null) {
				var box = $("<div class='Aw3_lightbox'> " + 
							"<a href='#' class='Aw3_close'></a>" + 
							"<div class='Aw3_innerbox'>" + options.content + "</div>" +
						"</div>");	
			}
			else if (options.content !== null){
				var box = $("<div class='Aw3_lightbox'> " + 
							"<a href='#' class='Aw3_close'></a>" + 
							"<div class='Aw3_innerbox'><h2>' " + options.title + " '</h2><p>' " + options.content + " '</p></div>" +
						"</div>");	
			}
			else {
				var box = $("<div class='Aw3_lightbox'> " + 
							"<a href='#' class='Aw3_close'></a>" + 
							"<div class='Aw3_innerbox'><h5>Empty lightbox (need to set options.content property)</h5></p></div>" +
						"</div>");
			}

			$(box).appendTo('.Aw3_overlay');
			
			$(".Aw3_close").click(function(event) {
				$(this).parent().fadeOut().remove();
				$('.Aw3_overlay').fadeOut().remove();
			});
			
		}

		return this;

	}


})($);