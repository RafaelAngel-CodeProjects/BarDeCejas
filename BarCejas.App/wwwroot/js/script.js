(function ($) {
  'use strict';

  /* ========================================================================= */
  /*	Page Preloader
  /* ========================================================================= */

  // window.load = function () {
  // 	document.getElementById('preloader').style.display = 'none';
  // }

  $(window).on('load', function () {
    $('#preloader').fadeOut('slow', function () {
      $(this).remove();
    });
  });

  
  //Hero Slider
  $('.hero-slider').slick({
    autoplay: true,
    infinite: true,
    arrows: true,
    prevArrow: '<button type=\'button\' class=\'prevArrow\'></button>',
    nextArrow: '<button type=\'button\' class=\'nextArrow\'></button>',
    dots: false,
    autoplaySpeed: 7000,
    pauseOnFocus: false,
    pauseOnHover: false
  });
  $('.hero-slider').slickAnimation();


  /* ========================================================================= */
  /*	Header Scroll Background Change
  /* ========================================================================= */

  $(window).scroll(function () {
    var scroll = $(window).scrollTop();
    //console.log(scroll);
    if (scroll > 200) {
      //console.log('a');
      $('.navigation').addClass('sticky-header');
    } else {
      //console.log('a');
      $('.navigation').removeClass('sticky-header');
    }
  });

  /* ========================================================================= */
  /*	Portfolio Filtering Hook
  /* =========================================================================  */

    // filter
    setTimeout(function(){
      var containerEl = document.querySelector('.filtr-container');
      var filterizd;
      if (containerEl) {
        filterizd = $('.filtr-container').filterizr({});
      }
    }, 500);

  /* ========================================================================= */
  /*	Testimonial Carousel
  /* =========================================================================  */

  //Init the slider
  $('.testimonial-slider').slick({
    infinite: true,
    arrows: false,
    autoplay: true,
    autoplaySpeed: 2000
  });


  /* ========================================================================= */
  /*	Clients Slider Carousel
  /* =========================================================================  */

  //Init the slider
  $('.clients-logo-slider').slick({
    infinite: true,
    arrows: false,
    autoplay: true,
    autoplaySpeed: 2000,
    slidesToShow: 5,
    slidesToScroll: 1
  });




  /* ========================================================================= */
  /*	Company Slider Carousel
  /* =========================================================================  */
  $('.company-gallery').slick({
    infinite: true,
    arrows: false,
    autoplay: true,
    autoplaySpeed: 2000,
    slidesToShow: 5,
    slidesToScroll: 1
  });


  /* ========================================================================= */
  /*   Contact Form Validating
  /* ========================================================================= */

  $('#contact-form').validate({
      rules: {
        name: {
          required: true,
          minlength: 4
        },
        email: {
          required: true,
          email: true
        },
        subject: {
          required: false
        },
        message: {
          required: true
        }
      },
      messages: {
        user_name: {
          required: 'Come on, you have a name don\'t you?',
          minlength: 'Your name must consist of at least 2 characters'
        },
        email: {
          required: 'Please put your email address'
        },
        message: {
          required: 'Put some messages here?',
          minlength: 'Your name must consist of at least 2 characters'
        }
      },
      submitHandler: function (form) {
        $(form).ajaxSubmit({
          type: 'POST',
          data: $(form).serialize(),
          url: 'sendmail.php',
          success: function () {
            $('#contact-form #success').fadeIn();
          },
          error: function () {
            $('#contact-form #error').fadeIn();
          }
        });
      }
    }

  );

  /* ========================================================================= */
  /*	On scroll fade/bounce effect
  /* ========================================================================= */
  var scroll = new SmoothScroll('a[href*="#"]');

  // -----------------------------
  //  Count Up
  // -----------------------------
  function counter() {
    if ($('.counter').length !== 0) {
      var oTop = $('.counter').offset().top - window.innerHeight;
    }
    if ($(window).scrollTop() > oTop) {
      $('.counter').each(function () {
        var $this = $(this),
          countTo = $this.attr('data-count');
        $({
          countNum: $this.text()
        }).animate({
          countNum: countTo
        }, {
          duration: 1000,
          easing: 'swing',
          step: function () {
            $this.text(Math.floor(this.countNum));
          },
          complete: function () {
            $this.text(this.countNum);
          }
        });
      });
    }
    }
     // -----------------------------
  //upload image
  // -----------------------------

    const $seleccionArchivos = document.querySelector("#seleccionArchivos");
    if ($seleccionArchivos!=null) {

    $seleccionArchivos.addEventListener("change", () => {
        // Los archivos seleccionados, pueden ser muchos o uno
        const archivos = $seleccionArchivos.files;
        var bgImg = $('#imagenPrevisualizacion').css('background-image')
        // Si no hay archivos salimos de la función y quitamos la imagen
        if (!archivos || !archivos.length) {

            $('#imagenPrevisualizacion').css("background-image", "url( " + bgImg + ")");
            return;
        }
        // Ahora tomamos el primer archivo, el cual vamos a previsualizar
        const primerArchivo = archivos[0];
        // Lo convertimos a un objeto de tipo objectURL
        const objectURL = URL.createObjectURL(primerArchivo);
        // Y a la fuente de la imagen le ponemos el objectURL

        $('#imagenPrevisualizacion').css("background-image", "url( " + objectURL + ")");
    });

    }

    const $seleccionArchivosSecon = document.querySelector("#seleccionArchivosSecon");
    if ($seleccionArchivosSecon != null) {
        // 2 imagen
   
    $seleccionArchivosSecon.addEventListener("change", () => {
        // Los archivos seleccionados, pueden ser muchos o uno
        const archivos = $seleccionArchivosSecon.files;
        var bgImg = $('#imagenPrevisualizacionSecon').css('background-image')
        // Si no hay archivos salimos de la función y quitamos la imagen
        if (!archivos || !archivos.length) {

            $('#imagenPrevisualizacionSecon').css("background-image", "url( " + bgImg + ")");
            return;
        }
        // Ahora tomamos el primer archivo, el cual vamos a previsualizar
        const primerArchivo = archivos[0];
        // Lo convertimos a un objeto de tipo objectURL
        const objectURL = URL.createObjectURL(primerArchivo);
        // Y a la fuente de la imagen le ponemos el objectURL

        $('#imagenPrevisualizacionSecon').css("background-image", "url( " + objectURL + ")");
    });
    }
    const $seleccionArchivosTri = document.querySelector("#seleccionArchivosTri");
    if ($seleccionArchivosTri != null) {
        // 3 image
   
    $seleccionArchivosTri.addEventListener("change", () => {
        // Los archivos seleccionados, pueden ser muchos o uno
        const archivos = $seleccionArchivosTri.files;
        var bgImg = $('#imagenPrevisualizacionTri').css('background-image')
        // Si no hay archivos salimos de la función y quitamos la imagen
        if (!archivos || !archivos.length) {

            $('#imagenPrevisualizacionTri').css("background-image", "url( " + bgImg + ")");
            return;
        }
        // Ahora tomamos el primer archivo, el cual vamos a previsualizar
        const primerArchivo = archivos[0];
        // Lo convertimos a un objeto de tipo objectURL
        const objectURL = URL.createObjectURL(primerArchivo);
        // Y a la fuente de la imagen le ponemos el objectURL

        $('#imagenPrevisualizacionTri').css("background-image", "url( " + objectURL + ")");
    });
    }
    const $seleccionArchivosFor = document.querySelector("#seleccionArchivosFor");

    if ($seleccionArchivosFor != null) {
    //4 imagen
   
    $seleccionArchivosFor.addEventListener("change", () => {
        // Los archivos seleccionados, pueden ser muchos o uno
        const archivos = $seleccionArchivosFor.files;
        var bgImg = $('#imagenPrevisualizacionFor').css('background-image')
        // Si no hay archivos salimos de la función y quitamos la imagen
        if (!archivos || !archivos.length) {

            $('#imagenPrevisualizacionFor').css("background-image", "url( " + bgImg + ")");
            return;
        }
        // Ahora tomamos el primer archivo, el cual vamos a previsualizar
        const primerArchivo = archivos[0];
        // Lo convertimos a un objeto de tipo objectURL
        const objectURL = URL.createObjectURL(primerArchivo);
        // Y a la fuente de la imagen le ponemos el objectURL

        $('#imagenPrevisualizacionFor').css("background-image", "url( " + objectURL + ")");
    });
        }
            // -----------------------------
  //  On Scroll
  // -----------------------------
  $(window).scroll(function () {
    counter();
  });

})(jQuery);
