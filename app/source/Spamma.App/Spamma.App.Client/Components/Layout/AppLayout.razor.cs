﻿using BlazorBootstrap;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;

namespace Spamma.App.Client.Components.Layout;

public partial class AppLayout()
{
    IEnumerable<NavItem>? navItems;

    private async Task<Sidebar2DataProviderResult> Sidebar2DataProvider(Sidebar2DataProviderRequest request)
    {
        if (this.navItems is null)
        {
            this.navItems = this.GetNavItems();
        }

        await Task.Delay(2000);

        return await Task.FromResult(request.ApplyTo(this.navItems));
    }

    private IEnumerable<NavItem> GetNavItems()
    {
        this.navItems = new List<NavItem>
        {
            new NavItem { Id = "1", Href = "/getting-started", IconName = IconName.HouseDoorFill, Text = "Getting Started"},

            new NavItem { Id = "2", IconName = IconName.LayoutSidebarInset, Text = "Content" },
            new NavItem { Id = "3", Href = "/icons", IconName = IconName.PersonSquare, Text = "Icons", ParentId="2"},

            new NavItem { Id = "4", IconName = IconName.ExclamationTriangleFill, Text = "Components" },
            new NavItem { Id = "5", Href = "/alerts", IconName = IconName.CheckCircleFill, Text = "Alerts", ParentId="4"},
            new NavItem { Id = "6", Href = "/breadcrumb", IconName = IconName.SegmentedNav, Text = "Breadcrumb", ParentId="4"},

            new NavItem { Id = "7", IconName = IconName.ListNested, Text = "Sidebar 2", ParentId="4"},
            new NavItem { Id = "701", Href = "/sidebar2", IconName = IconName.Dash, Text = "How to use", ParentId="7"},
            new NavItem { Id = "702", Href = "/sidebar2-examples", IconName = IconName.Dash, Text = "Examples", ParentId="7"},

            new NavItem { Id = "8", IconName = IconName.WindowPlus, Text = "Forms" },
            new NavItem { Id = "9", Href = "/autocomplete", IconName = IconName.InputCursorText, Text = "Auto Complete", ParentId="8"},
            new NavItem { Id = "10", Href = "/currency-input", IconName = IconName.CurrencyDollar, Text = "Currency Input", ParentId="8"},
            new NavItem { Id = "11", Href = "/number-input", IconName = IconName.InputCursor, Text = "Number Input", ParentId="8"},
            new NavItem { Id = "12", Href = "/switch", IconName = IconName.ToggleOn, Text = "Switch", ParentId="8"},
        };

        return this.navItems;
    }
}