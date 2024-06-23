(function ($) {
    "use strict";
    var windowOn = $(window);

    /*-----------------------------------------------------------------------------------
        Template Name: Medilix - Healthcare & Medical Bootstrap HTML5 Template
        Author: RRDevs
        Support: https://support.rrdevs.net
        Description: Medilix - Healthcare & Medical Bootstrap HTML5 Template.
        Version: 1.0
        Developer: Sabbir Ahmmed (https://github.com/ahmmedsabbirbd)
    -----------------------------------------------------------------------------------

     */
   /*======================================
   Data Css js
   ========================================*/
    $("[data-background]").each(function() {
        $(this).css(
            "background-image",
            "url( " + $(this).attr("data-background") + "  )"
        );
    });

    $("[data-width]").each(function() {
        $(this).css("width", $(this).attr("data-width"));
    });

    class GSAPAnimation {
        static Init() {
            /*title-animation*/
            $('.title-animation').length && this.sectionTitleAnimation('.title-animation'); 
        }
        
        static sectionTitleAnimation(activeClass) {
            let sectionTitleLines = gsap.utils.toArray(activeClass);

            sectionTitleLines.forEach(sectionTextLine => {
                const tl = gsap.timeline({
                    scrollTrigger: {
                        trigger: sectionTextLine,
                        start: 'top 90%',
                        end: 'bottom 60%',
                        scrub: false,
                        markers: false,
                        toggleActions: 'play none none none'
                    }
                });

                const itemSplitted = new SplitText(sectionTextLine, { type: "chars, words" });
                gsap.set(sectionTextLine, { perspective: 100 });
                itemSplitted.split({ type: "words" })
                tl.from(itemSplitted.words, {
                    opacity: 0, 
                    autoAlpha: 0, 
                    transformOrigin: "top center -50",
                    y: "10px",
                    duration: 0.9,
                    stagger: 0.1,
                    ease: "power2.out",
                });
            });
        }
    }

    class RRDEVS {
        static LoadedAfter() {
            $('#preloader').delay(1).fadeOut(0);

            $(".odometer").waypoint(
                function () {
                    var odo = $(".odometer");
                    odo.each(function () {
                        var countNumber = $(this).attr("data-count");
                        var element = $(this);
                        setTimeout(function() {
                            element.html(countNumber);
                        }, 1000); // 1000 milliseconds delay (1 second)
                    });
                },
                {
                    offset: "80%",
                    triggerOnce: true,
                }
            );

            /*Wow Js*/
            if ($('.wow').length) {
                var wow = new WOW({
                    boxClass: 'wow',
                    animateClass: 'animated',
                    offset: 0,
                    mobile: false,
                    live: true
                });
                wow.init();
            }

            /*GSAPAnimation*/
            GSAPAnimation.Init();
        }
    }

    /*======================================
      Preloader activation
      ========================================*/
    $(window).on('load', RRDEVS.LoadedAfter);
    $(".preloader-close").on("click",  RRDEVS.LoadedAfter)

    window.addEventListener('resize', function() {
        gsap.globalTimeline.clear();
    });

    /*======================================
      Mobile Menu Js
      ========================================*/
    $("#mobile-menu").meanmenu({
        meanMenuContainer: ".mobile-menu",
        meanScreenWidth: "1199",
        meanExpand: ['<i class="fa-regular fa-angle-right"></i>'],
    });

    /*======================================
      Sidebar Toggle
      ========================================*/
    $(".offcanvas__close,.offcanvas__overlay").on("click", function () {
        $(".offcanvas__area").removeClass("info-open");
        $(".offcanvas__overlay").removeClass("overlay-open");
    });
    // Scroll to bottom then close navbar
    $(window).scroll(function(){
        if($("body").scrollTop() > 0 || $("html").scrollTop() > 0) {
            $(".offcanvas__area").removeClass("info-open");
            $(".offcanvas__overlay").removeClass("overlay-open");
        }
    });
    $(".sidebar__toggle").on("click", function () {
        $(".offcanvas__area").addClass("info-open");
        $(".offcanvas__overlay").addClass("overlay-open");
    });

    /*======================================
      Body overlay Js
      ========================================*/
    $(".body-overlay").on("click", function () {
        $(".offcanvas__area").removeClass("opened");
        $(".body-overlay").removeClass("opened");
    });

    /*======================================
      Sticky Header Js
      ========================================*/
    $(window).scroll(function () {
        if ($(this).scrollTop() > 10) {
            $("#header-sticky").addClass("rr-sticky");
        } else {
            $("#header-sticky").removeClass("rr-sticky");
        }
    });

    /*======================================
      MagnificPopup image view
      ========================================*/
    $(".popup-image").magnificPopup({
        type: "image",
        gallery: {
            enabled: true,
        },
    });

    /*======================================
      MagnificPopup video view
      ========================================*/
    $(".popup-video").magnificPopup({
        type: "iframe",
    });

    /*======================================
      Page Scroll Percentage
      ========================================*/
    const scrollTopPercentage = ()=> {
        const scrollPercentage = () => {
            const scrollTopPos = document.documentElement.scrollTop;
            const calcHeight = document.documentElement.scrollHeight - document.documentElement.clientHeight;
            const scrollValue = Math.round((scrollTopPos / calcHeight) * 100);
            const scrollElementWrap = $("#scroll-percentage");

            scrollElementWrap.css("background", `conic-gradient( var(--rr-theme-primary) ${scrollValue}%, var(--rr-color-900) ${scrollValue}%)`);

            if ( scrollTopPos > 100 ) {
                scrollElementWrap.addClass("active");
            } else {
                scrollElementWrap.removeClass("active");
            }

            if( scrollValue < 96 ) {
                $("#scroll-percentage-value").text(`${scrollValue}%`);
            } else {
                $("#scroll-percentage-value").html('<i class="fa-solid fa-angle-up"></i>');
            }
        }
        window.onscroll = scrollPercentage;
        window.onload = scrollPercentage;

        // Back to Top
        function scrollToTop() {
            document.documentElement.scrollTo({
                top: 0,
                behavior: "smooth"
            });
        }

        $("#scroll-percentage").on("click", scrollToTop);
    }
    scrollTopPercentage();

    /*======================================
	One Page Scroll Js
	========================================*/
    var link = $('.onepagenav #mobile-menu ul li a, .onepagenav .mean-nav ul li a');
    link.on('click', function(e) {
        var target = $($(this).attr('href'));
        $('html, body').animate({
            scrollTop: target.offset().top - 76
        }, 600);
        $(this).parent().addClass('active');
        e.preventDefault();
    });
    $(window).on('scroll', function(){
        scrNav();
    });

    function scrNav() {
        var sTop = $(window).scrollTop();
        $('section').each(function() {
            var id = $(this).attr('id'),
                offset = $(this).offset().top-1,
                height = $(this).height();
            if(sTop >= offset && sTop < offset + height) {
                link.parent().removeClass('active');
                $('.main-menu').find('[href="#' + id + '"]').parent().addClass('active');
            }
        });
    }
    scrNav();

    /*======================================
	Smoth animatio Js
	========================================*/
    $(document).on('click', '.smoth-animation', function (event) {
        event.preventDefault();
        $('html, body').animate({
            scrollTop: $($.attr(this, 'href')).offset().top - 50
        }, 300);
    });

    /*brand__active***/
    let brand = new Swiper(".brand__active", {
        slidesPerView: 1,
        spaceBetween: 156,
        loop: true,
        roundLengths: true,
        clickable: true,
        autoplay: {
            delay: 3000,
        },
        breakpoints: {
            1401: {
                slidesPerView: 5,
            },
            1200: {
                slidesPerView: 4,
            },
            992: {
                slidesPerView: 3,
            },
            576: {
                spaceBetween: 30,
                slidesPerView: 3,
            },
            481: {
                slidesPerView: 2,
            },
            0: {
                slidesPerView: 1,
            },
        },
    });

    /*testimonial__slider***/
    let header3TopSlider = new Swiper(".testimonial__slider", {
        slidesPerView: 1,
        spaceBetween: 30,
        loop: true,
        navigation: {
            prevEl: ".testimonial__slider__arrow-prev",
            nextEl: ".testimonial__slider__arrow-next",
        },
        clickable: true,
        autoplay: {
            delay: 3000,
        }
    });

    /*client-testimonial__slider***/
    let clienttestimonial__slider = new Swiper(".client-testimonial__slider", {
        slidesPerView: 1,
        spaceBetween: 30,
        loop: true,
        clickable: true,
        pagination: {
            el: ".client-testimonial__slider-dot",
            clickable: true,
        },
        autoplay: {
            delay: 3000,
        }
    });

    /*doctor__slider***/
    let doctor__slider = new Swiper(".doctor__slider", {
        slidesPerView: 1,
        spaceBetween: 30,
        loop: true,
        clickable: true,
        pagination: {
            el: ".doctor__slider-dot",
            clickable: true,
        },
        autoplay: {
            delay: 3000,
        },
        breakpoints: {
            1200: {
                slidesPerView: 3,
            },
            768: {
                slidesPerView: 2,
            },
            0: {
                slidesPerView: 1,
            },
        },
    });

    /*latest-work__slider***/
    let latest_work__slider = new Swiper(".latest-work__slider", {
        slidesPerView: 1,
        spaceBetween: 30,
        loop: true,
        clickable: true,
        centeredSlides: true,
        pagination: {
            el: ".latest-work__slider-dot",
            clickable: true,
        },
        autoplay: {
            delay: 3000,
        },
        breakpoints: {
            768: {
                slidesPerView: 2,
            },
            0: {
                slidesPerView: 1,
            },
        },
    });

    /*banner-3__slider***/
    let Banner3Slider = new Swiper(".banner-3__slider", {
        slidesPerView: 1,
        spaceBetween: 30,
        loop: true,
        clickable: true,
        autoplay: {
            delay: 3000,
        },
        pagination: {
            clickable: true,
            el: ".banner-3__pagination",
            renderBullet: function (index, className) {
                let indexText = (index + 1 < 10) ? '0' + (index + 1) : (index + 1);
                return '<span class="' + className + '">' + indexText + "</span>";
            },
        },
    });

    /*blog-2__slider***/
    let blog2__slider = new Swiper(".blog-2__slider", {
        slidesPerView: 1,
        spaceBetween: 30,
        loop: true,
        navigation: {
            prevEl: ".blog-2__slider__arrow-prev",
            nextEl: ".blog-2__slider__arrow-next",
        },
        clickable: true,
        autoplay: {
            delay: 3000,
        },
        breakpoints: {
            1200: {
                slidesPerView: 3,
            },
            768: {
                slidesPerView: 2,
            },
            0: {
                slidesPerView: 1,
            },
        },
    });

    /*testimonial-2__slider***/
    let testimonial2__slider = new Swiper(".testimonial-2__slider", {
        slidesPerView: 1,
        spaceBetween: 30,
        loop: true,
        navigation: {
            prevEl: ".testimonial-2__slider__arrow-prev",
            nextEl: ".testimonial-2__slider__arrow-next",
        },
        clickable: true,
        autoplay: {
            delay: 3000,
        }
    });

    /*service-2__slider***/
    let service2__slider = new Swiper(".service-2__slider", {
        slidesPerView: 1,
        spaceBetween: 30,
        loop: true,
        navigation: {
            prevEl: ".service-2__slider__arrow-prev",
            nextEl: ".service-2__slider__arrow-next",
        },
        clickable: true,
        autoplay: {
            delay: 3000,
        },
        breakpoints: {
            1200: {
                slidesPerView: 3,
            },
            768: {
                slidesPerView: 2,
            },
            0: {
                slidesPerView: 1,
            },
        },
    });

    /*team__slider***/
    let team__slider = new Swiper(".team__slider", {
        slidesPerView: 1,
        spaceBetween: 40,
        loop: false,
        roundLengths: true,
        clickable: true,
        scrollbar: {
            el: ".team__scrollbar",
            hide: true,
        },
        autoplay: {
            delay: 3000,
        },
        breakpoints: {
            1200: {
                slidesPerView: 3,
            },
            768: {
                slidesPerView: 2,
            },
            0: {
                slidesPerView: 1,
            },
        },
    });

    //slider-text
    const scrollers = document.querySelectorAll(".rr-scroller");
    if (!window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
        addAnimation();
    }
    function addAnimation() {
        scrollers.forEach((scroller) => {
            scroller.setAttribute("data-animated", true);

            const scrollerInner = scroller.querySelector(".rr-scroller__inner");
            const scrollerContent = Array.from(scrollerInner.children);

            scrollerContent.forEach((item) => {
                const duplicatedItem = item.cloneNode(true);
                duplicatedItem.setAttribute("aria-hidden", true);
                scrollerInner.appendChild(duplicatedItem);
            });
        });
    }

    /* lastNobullet ***/
    function lastNobullet() {
        var lastElement = false;
        $(".footer__copyright-menu ul li, .last_item_not_horizental_bar .col-lg-4").each(function() {
            if (lastElement && lastElement.offset().top != $(this).offset().top) {
                $(lastElement).addClass("no_bullet");
            } else {
                $(lastElement).removeClass("no_bullet");
            }
            lastElement = $(this);
        }).last().addClass("no_bullet");
    };
    lastNobullet();

    $(window).resize(function(){
        lastNobullet();
    });

    $('#pricing-appointment__form').submit(function(event) {
        event.preventDefault();
        var form = $(this);
        $('.loading-form').show();

        setTimeout(function() {
            $.ajax({
                type: form.attr('method'),
                url: form.attr('action'),
                data: form.serialize()
            }).done(function(data) {
                $('.loading-form').hide();
                $('.pricing-appointment__form').append('<p class="success-message mt-3 mb-0">Your message has been sent successfully.</p>');
            }).fail(function(data) {
                $('.loading-form').hide();
                $('.pricing-appointment__form').append('<p class="error-message mt-3 mb-0">Something went wrong. Please try again later.</p>');

            });
        }, 1000);
    });

    $('.take-appointment-3__form-input-select select, .take-appointment__form-input-select select, .doctor-details__form-input-select select, .appointment-2__form-input-select select, .pricing-appointment__form-select select').niceSelect();
    $( "#datepicker" ).datepicker({
        dateFormat: "yy/mm/dd"
    });

    $(".search-open-btn").on("click", function () {
        $(".search__popup").addClass("search-opened");
    });

    $(".search-close-btn").on("click", function () {
        $(".search__popup").removeClass("search-opened");
    });

    /* Popular Causes Progress Bar ***/
    if ($(".count-bar").length) {
        $(".count-bar").appear(
            function() {
                var el = $(this);
                var percent = el.data("percent");
                $(el).css("width", percent).addClass("counted");
            }, {
                accY: -50
            }
        );
    }


    /* image compare js ***/
    var ctrl = jQuery('.filter__container .comparison-ctrl');
    var pic_right = jQuery('.filter__container .pic--right');
    Draggable.create(ctrl,{
        bounds: ctrl.parent(),
        type: "x",
        onDrag: function(){
            pic_right.css('left','calc(50% + '+(this.x - 2)+'px)');
        }
    });

    /*specialist-doctor__slider***/
    let specialist = new Swiper(".specialist-doctor__slider", {
        slidesPerView: 1,
        spaceBetween: 30,
        loop: false,
        roundLengths: true,
        clickable: true,
        centerMode: true,
        scrollbar: {
            el: ".specialist-doctor__scrollbar ",
            hide: true,
        },
        autoplay: {
            delay: 3000,
        },
        breakpoints: {
            1200: {
                slidesPerView: 4,
            },
            768: {
                slidesPerView: 3,
            },
            575: {
                slidesPerView: 2,
            },
            0: {
                slidesPerView: 1,
            },
        },
    });

})(jQuery);

