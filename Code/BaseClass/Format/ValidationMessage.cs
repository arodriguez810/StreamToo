using System.Collections.Generic;

namespace Admin.BaseClass.Format
{
    public class ValidationMessage
    {
        Dictionary<string, string> messages = new Dictionary<string, string>();
        public ValidationMessage()
        {
            messages.Add("strongPassword", "The password must be equal to or greater than 8 characters and contain at least one letter, one number");
            messages.Add("required", "This field is required.");
            messages.Add("remote", "a record already exists with this {0}.");
            messages.Add("remotePlus", "Already a combination between the fields {0}.");
            messages.Add("precision", "The integer part of this number must be between 1 and {0}.");
            messages.Add("scale", "The decimal part of this number must be between 1 and {0}.");
            messages.Add("email", "Please enter a valid email");
            messages.Add("url", " Please enter a valid URL. ");
            messages.Add("date", " Please enter a valid date. ");
            messages.Add("dateiso", "Please enter a date (ISO) valid. ");
            messages.Add("number", "Please enter a valid integer . ");
            messages.Add("digits", "Please enter only digits. ");
            messages.Add("creditcard ", "Please enter a valid card number . ");
            messages.Add("equalto", "Please enter the same value again. ");
            messages.Add("accept", "Please enter a value with a extension accepted. ");
            messages.Add("maxlength", "Please do not enter more than {0} characters. ");
            messages.Add("minLength", "Please do not write less than {0} characters. ");
            messages.Add("rangelength", "Please enter a value between {0} and {1} characters. ");
            messages.Add("range", "Please enter a value between {0} and {1} . ");
            messages.Add("max", "Please enter a value less than or equal to {0}. ");
            messages.Add("min", "Please enter a value greater than or equal to {0}. ");
            messages.Add("Yes", "Yes");
            messages.Add("No", "No");
            messages.Add("Clear Local Storage", "Clear Local Storage");
            messages.Add("Logout", "Logout");
            messages.Add("No matches found", "No matches found");
            messages.Add(" more character", " more character");
            messages.Add("Please enter ", "Please enter ");
            messages.Add("Please delete ", "Please delete ");
            messages.Add(" character", " character");
            messages.Add("You can only select ", "You can only select ");
            messages.Add(" item", " item");
            messages.Add("Loading more results...", "Loading more results...");

        }

    }
}