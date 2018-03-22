import * as ko from 'knockout';



interface ISearchResult {

    searchEngineResults: ISearchEngineResult[];
}

interface ISearchEngineResult {
    searchEngineValues: ISearchEngineValueReturned[];
    aggregate: number;
}
interface ISearchEngineValueReturned {
    searchEngineName :string ;
    searchEngineOriginType:number;
    counter: number;
    textQuery :string;
}


class SearchMachinev2ViewModel {
    public searchText = ko.observable();
    public searchResult = ko.observable();
    public searchResultJson = ko.observable();

    public searchMachinev2Action() {
        var textquery = this.searchText();
        fetch('api/SearchOne/ResultSearch', {
                method: 'post',
                body: JSON.stringify(textquery),
                headers: new Headers({
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                })
            })
            .then(response => response.json() as Promise<ISearchResult>)
                .then(searchResult => {
                    console.log('data returned', searchResult);
                    console.log('data returned formated json',JSON.stringify(searchResult));
                    console.log('data returned formated', JSON.parse(ko.toJSON(searchResult)));
                    this.searchResultJson(ko.toJSON(searchResult));
                    this.searchResult(searchResult);
                    console.log('Observable ', this.searchResult());
               
            });
            
    }


}

export default { viewModel: SearchMachinev2ViewModel, template: require('./search-machinev2.html') };
