using System;
using System.Reactive.Linq;
using Camelot.Extensions;
using Camelot.Services.Abstractions.Models;
using Camelot.Services.Abstractions.Specifications;
using Camelot.Services.Environment.Interfaces;
using Camelot.ViewModels.Configuration;
using Camelot.ViewModels.Implementations.MainWindow.FilePanels.Specifications;
using Camelot.ViewModels.Interfaces.MainWindow.FilePanels;
using Camelot.ViewModels.Services.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Camelot.ViewModels.Implementations.MainWindow.FilePanels
{
    public class SearchViewModel : ValidatableViewModelBase<SearchViewModel>, ISearchViewModel
    {
        private readonly IRegexService _regexService;

        private bool IsValid => !IsRegexSearchEnabled || _regexService.ValidateRegex(SearchText);

        [Reactive]
        public string SearchText { get; set; }

        [Reactive]
        public bool IsSearchCaseSensitive { get; set; }

        [Reactive]
        public bool IsRegexSearchEnabled { get; set; }

        [Reactive]
        public bool IsSearchEnabled { get; set; }

        public event EventHandler<EventArgs> SearchSettingsChanged;

        public SearchViewModel(
            IRegexService regexService,
            IResourceProvider resourceProvider,
            SearchViewModelConfiguration searchViewModelConfiguration)
        {
            _regexService = regexService;
            Reset();

            this.ValidationRule(vm => vm.SearchText,
                vm =>
                    this.WhenAnyValue(x => x.IsRegexSearchEnabled, x => x.SearchText).Select(_ => IsValid),
                (vm, r) => resourceProvider.GetResourceByName(searchViewModelConfiguration.InvalidRegexResourceName));
            this.WhenAnyValue(x => x.SearchText, x => x.IsSearchEnabled,
                    x => x.IsRegexSearchEnabled, x => x.IsSearchCaseSensitive)
                .Throttle(TimeSpan.FromMilliseconds(searchViewModelConfiguration.TimeoutMs))
                .Subscribe(_ => FireSettingsChangedEvent());
        }

        public ISpecification<NodeModelBase> GetSpecification() =>
            (IsSearchEnabled, IsRegexSearchEnabled) switch
            {
                (true, true) when IsValid => new NodeNameRegexSpecification(_regexService, SearchText, IsSearchCaseSensitive),
                (true, false) => new NodeNameTextSpecification(SearchText, IsSearchCaseSensitive),
                _ => new EmptySpecification()
            };

        public void ToggleSearch()
        {
            Reset();
            IsSearchEnabled = !IsSearchEnabled;
        }

        private void Reset() => SearchText = string.Empty;

        private void FireSettingsChangedEvent()
        {
            if (IsValid)
            {
                SearchSettingsChanged.Raise(this, EventArgs.Empty);
            }
        }
    }
}