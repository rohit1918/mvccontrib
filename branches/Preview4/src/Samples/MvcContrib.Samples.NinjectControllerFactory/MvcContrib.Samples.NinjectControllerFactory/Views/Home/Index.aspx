<%@ Page Language="C#" AutoEventWireup="false" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
            <title>Ninject ControllerFactory</title>
    </head>
    <body>
            <div>
                Attack result by Samurai injected using Ninject:
                <%= ViewData["attackresult"] %>               
            </div>
    </body>
</html>
