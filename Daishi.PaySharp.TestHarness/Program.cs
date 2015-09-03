﻿#region Includes

using System;
using System.Configuration;
using System.Text;

#endregion

namespace Daishi.PaySharp.TestHarness {
    internal class Program {
        private static void Main(string[] args) {

            Console.Write("Press the <return> key to run...");
            Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Executing SETEXPRESSCHECKOUT...");
            Console.ResetColor();

            try {
                var user = ConfigurationManager.AppSettings["User"];
                var password = ConfigurationManager.AppSettings["Password"];
                var signature = ConfigurationManager.AppSettings["Signature"];

                var payPalAdapter = new PayPalAdapter();

                #region SETEXPRESSCHECKOUT

                var setExpresscheckout = payPalAdapter.SetExpressCheckout(new SetExpressCheckoutPayload {
                    User = user,
                    Password = password,
                    Signature = signature,
                    Method = "SetExpressCheckout",
                    Version = "78",
                    Amount = "19",
                    CurrencyCode = "USD",
                    CancelUrl = "http://www.example.com/cancel.html",
                    ReturnUrl = "http://www.example.com/success.html"
                },
                    Encoding.UTF8, ConfigurationManager.AppSettings["ExpressCheckoutURI"]);

                string accessToken;
                PayPalError payPalError;

                var ok = PayPalUtility.TryParseAccessToken(setExpresscheckout, out accessToken, out payPalError);

                if (ok) {
                    Console.Write("SETEXPRESSCHECKOUT: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK");
                    Console.ResetColor();

                    Console.WriteLine();
                    Console.Write("PayPal Access Token: ");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(accessToken);

                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine();
                }
                else {
                    Console.Write("SETEXPRESSCHECKOUT: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FAIL");
                    Console.ResetColor();

                    Console.WriteLine();
                    Console.Write("PayPal Short Error Message: ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(payPalError.ShortMessage);

                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Press any key to quit...");
                    Console.ReadLine();
                    return;
                }

                #endregion

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Executing GETEXPRESSCHECKOUTDETAILS...");
                Console.ResetColor();

                #region GETEXPRESSCHECKOUTDETAILS

                var getExpressCheckoutDetails = payPalAdapter.GetExpressCheckoutDetails(
                    new GetExpressCheckoutDetailsPayload {
                        User = ConfigurationManager.AppSettings["User"],
                        Password = ConfigurationManager.AppSettings["Password"],
                        Signature = ConfigurationManager.AppSettings["Signature"],
                        Method = "GetExpressCheckoutDetails",
                        Version = "93",
                        AccessToken = accessToken
                    }, ConfigurationManager.AppSettings["ExpressCheckoutURI"]);

                CustomerDetails customerDetails;

                ok = PayPalUtility.TryParseCustomerDetails(
                    getExpressCheckoutDetails, out customerDetails, out payPalError);

                if (ok) {
                    Console.Write("GETEXPRESSCHECKOUTDETAILS: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK");
                    Console.ResetColor();

                    Console.WriteLine();
                    Console.Write("PayPal Acknowledgement: ");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(customerDetails.Ack);
                }
                else {
                    Console.Write("GETEXPRESSCHECKOUTDETAILS: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FAIL");
                    Console.ResetColor();

                    Console.WriteLine();
                    Console.Write("PayPal Short Error Message: ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(payPalError.ShortMessage);
                }

                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();

                #endregion

                Console.WriteLine("Press any key to quit...");
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
            }

            Console.ReadLine();
        }
    }
}