using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using GGApp.ViewModels;
using ReactiveUI;

namespace GGApp;

public class ViewLocator : IViewLocator, IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!.Replace("ViewModels", "Views").Replace("ViewModel", "", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
    private static readonly Dictionary<Type, Type> _routeTypes = new();

    private List<Type>? _cachedTypes;

    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        var type = viewModel.GetType();
        if (!_routeTypes.TryGetValue(type, out var pageType))
        {
            var findPageType = FindPageType(type);
            _routeTypes[type] = findPageType;
            pageType = findPageType;
        }

        var pageInstance = Activator.CreateInstance(pageType) as StyledElement;
        pageInstance.DataContext = viewModel;
        return pageInstance as IViewFor;
    }

    private Type FindPageType(Type type)
    {
        if (_cachedTypes is null)
        {
            var assembly = Assembly.GetAssembly(GetType());

            _cachedTypes = assembly.GetTypes().ToList();
        }

        var name = type.Name;
        if (name.EndsWith("ViewModel"))
            name = name[..^9];
        else if (name.EndsWith("VM")) name = name[..^2];

        var pageType = _cachedTypes.Find(i => i.Name == name + "Page");
        if (pageType is not null)
            return pageType;
        if (_cachedTypes.Find(i => i.Name == name) is { } justType) return justType;

        return null;
    }
}