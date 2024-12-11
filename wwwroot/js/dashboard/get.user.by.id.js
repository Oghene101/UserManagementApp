let editUserRoleLinks = document.querySelectorAll(".delete-user-role");

editUserRoleLinks.forEach((link) => {
    link.addEventListener("click", (event) => {
        event.preventDefault();

        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then(async (result) => {
            if (result.isConfirmed) {
                console.log("Deleted user role successfully.");
                let deleteUserRoleUrl = link.getAttribute("href");
                let response = await deleteUserRole(deleteUserRoleUrl);

                if (response !== "error") {
                    Swal.fire({
                        title: "Deleted!",
                        text: "Your file has been deleted.",
                        icon: "success"
                    });
                }
            }
        });

    });
});

async function deleteUserRole(url) {
    try {
        let response = await fetch(url, {
            method: "DELETE",
            headers: {
                "Accept": "application/json"
            }
        });

        if (!response.ok) {
            let errorResponse;

            try {
                errorResponse = await response.json();
            } catch (jsonError) {
                errorResponse = await response.text();
            }

            console.table(response);
            throw errorResponse;
        }

    } catch (error) {
        if (error && error.errors && Array.isArray(error.errors) && error.errors.length > 0) {
            error.errors.forEach((error) => {
                toastr.error((error.message === "The specified value is null.") ? "Role has been deleted." : error.message);
            });
        }
        console.table(error);
        return "error";
    }

}
