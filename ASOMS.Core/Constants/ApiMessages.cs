using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.Constants;
public static class ApiMessages
{
    // ✅ General
    public const string Ok = "Success";
    public const string InternalServerError = "An internal error occurred.";
    public const string Unauthorize = "Unauthorized action";
    public const string InvalidCredentials = "Invalid email or password.";
    public const string RequestNotProcessable = "Request could not be processed.";
    public const string EmailAlreadyUsed = "Email is already in use.";
    public const string ResourceNotFound = "Resource not found.";
    public const string ResourceAlreadyExists = "Resource already exists.";
    public const string InvalidLink = "The link is invalid.";
    public const string EntityNotFound = "The requested item does not exist.";

    // ✅ User-related
    public const string MemberEmailFound = "An account already exists with this email.";
    public const string MemberEmailNotFound = "No account found with this email.";
    public const string InvalidActivationCode = "Invalid activation code.";
    public const string MemberPendingEmailVerification = "Please verify your email before logging in.";

    // ✅ Product / Inventory / Order
    public const string ProductNotFound = "Product not found.";
    public const string ProductOutOfStock = "Product is currently out of stock.";
    public const string InventoryNotSufficient = "Insufficient stock for the requested quantity.";
    public const string OrderNotFound = "Order not found.";
    public const string OrderAlreadyCompleted = "Order has already been completed.";
    public const string CartIsEmpty = "Your cart is empty.";
    public const string CartItemNotFound = "Item not found in cart.";
    public const string OrderAlreadyExists = "This order has already been placed.";

    // ✅ File Upload
    public const string FileUploadFailed = "File upload failed.";
    public const string FileUploadAlreadyDeleted = "File has already been deleted.";
    public const string FileUploadPathNotFound = "Upload record found but file path is missing.";

    // ✅ Payment
    public const string PaymentFailed = "Payment failed. Please check your method.";
}

