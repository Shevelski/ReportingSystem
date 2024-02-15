$(document).ready(function () {
        // Function to load content dynamically
    function loadContent(url, target) {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (data) {
                $(target).html(data);
            },
            error: function () {
                alert('Error loading content');
            }
        });
    }

        // Handle navigation clicks
    $('.nav-link').on('click', function (e) {
        e.preventDefault();
        var url = $(this).attr('href');
        var target = $(this).data('target');
        loadContent(url, target);
        });

    $(document).ready(function () {
        $('.nav-link').click(function () {
            $('.nav-link').removeClass('active'); // Remove 'active' class from all items
            $(this).closest('.nav-link').addClass('active'); // Add 'active' class to the clicked item
        });
    });


    // Handle navigation clicks
    $('.navbar-brand').on('click', function (e) {
        e.preventDefault();
        var url = $(this).attr('href');
        var target = $(this).data('target');
        loadContent(url, target);
    });

    $(document).ready(function () {
        $('.navbar-brand').click(function () {
            $('.navbar-brand').removeClass('active'); // Remove 'active' class from all items
            $(this).closest('.navbar-brand').addClass('active'); // Add 'active' class to the clicked item
        });
    });
});


