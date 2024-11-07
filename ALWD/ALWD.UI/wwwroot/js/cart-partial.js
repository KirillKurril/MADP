document.getElementById("clearCartBtn").addEventListener("click", function () {
    fetch('@Url.Action("ClearCart", "Cart")', {
        method: 'DELETE',
        headers: {
            'X-CSRF-TOKEN': '@Antiforgery.GetTokens(HttpContext).RequestToken'
        }
    }).then(response => {
        if (response.ok) {
            window.location.reload();
        } else {
            alert("Failed to clear cart.");
        }
    });
});