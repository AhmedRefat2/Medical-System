$("button[data-type='delete']").on("click", function (e) {

    e.preventDefault();

    Swal.fire({
        title: 'Are you sure?',
        text: 'Are you sure, you want to delete this doctor?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'yes, delete!',
        cancelButtonText: 'No, Cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: "Deleted",
                text: "User Deleted Successfully.",
                icon: "success"
            })
                .then(_r => {
                    console.log((this.attributes.formaction))
                    console.log((this.attributes.formaction.value))
                    window.location.href = this.attributes.formaction.value;

                });
        }
    });
});


