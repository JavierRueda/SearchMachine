import * as ko from 'knockout';


interface IGoogleResult {
    counter: number;
    query: string;
}
interface IYahooResult {
    counter: number;
    query:string;
}
interface IBingResult {
    counter: number;
    query: string;
}

interface IGoGoDuckResult {
    counter: number;
    query: string;
}
interface IEcosiaResult {
    counter: number;
    query: string;
}

class SearchMachineViewModel {
    public searchText = ko.observable();
    public searchTextParams = ko.observable();

    googleResults = ko.observableArray<IGoogleResult>();
    yahooResults = ko.observableArray<IYahooResult>();
    bingResults = ko.observableArray<IYahooResult>();
    gogoduckResults = ko.observableArray<IGoGoDuckResult>();
    ecosiaResults = ko.observableArray<IEcosiaResult>();
    googleResultsSum = ko.observable(0);
    bingResultsSum = ko.observable(0);
    gogoduckResultsSum = ko.observable(0);
    ecosiaResultsSum = ko.observable(0);



    public searchMachineAction() {
        var value = this.searchText();
        console.log("search text:", value);
        var textparams = value.toString().split(' ');
        console.log("search text  params counter:", textparams.length);
        this.searchTextParams(textparams.length);
        this.googleResults.removeAll();
        this.yahooResults.removeAll();
        this.bingResults.removeAll();
        var sumGoogleMacths = 0;
        var sumBingMacths = 0;
        var sumGoGoDuckMacths = 0;
        for(var textquery of textparams) {
            console.log('textquery included in input', textquery);
             //Google
             fetch('api/Search/GoogleSearchWild', {
                    method: 'post',
                    body: JSON.stringify(textquery),
                    headers: new Headers({
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    })
                })
                .then(response => response.json() as Promise<IGoogleResult>)
                .then(googleResult => {
                    console.log('data returned', googleResult);
                    this.googleResults.push(googleResult);
                    sumGoogleMacths = parseInt(this.googleResultsSum().toString(), 0) + JSON.parse(ko.toJSON(googleResult)).counter; ;
                    this.googleResultsSum(sumGoogleMacths);
                 });
            //GoGoDuck
            fetch('api/Search/GoGoDuckSearch', {
                    method: 'post',
                    body: JSON.stringify(textquery),
                    headers: new Headers({
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    })
                })
                .then(response => response.json() as Promise<IGoGoDuckResult>)
                .then(gogoDuckResult => {
                    console.log('data returned', gogoDuckResult);
                    this.gogoduckResults.push(gogoDuckResult);
                    sumGoGoDuckMacths = parseInt(this.gogoduckResultsSum().toString(), 0) + JSON.parse(ko.toJSON(gogoDuckResult)).counter;;
                    this.gogoduckResultsSum(sumGoGoDuckMacths);
                 });
            //Bing
             fetch('api/Search/BingSearchWithKey', {
                    method: 'post',
                    body: JSON.stringify(textquery),
                    headers: new Headers({
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    })
                })
                .then(response => response.json() as Promise<IBingResult>)
                .then(bingResult => {
                    console.log('data returned', bingResult);
                    this.bingResults.push(bingResult);
                    sumBingMacths = parseInt(this.bingResultsSum().toString(), 0) + JSON.parse(ko.toJSON(bingResult)).counter;;
                     this.bingResultsSum(sumBingMacths);
                });
            //fetch('api/Search/BingSearch')
            //    .then(response => response.json() as Promise<YahooResults[]>)
            //    .then(data => {
            //        this.bingResults(data);
            //    });
            //fetch('api/Search/YahooSearch')
            //    .then(response => response.json() as Promise<YahooResults[]>)
            //    .then(data => {
            //        this.yahooResults(data);
            //    });
            //Ecosia
             fetch('api/Search/EcosiaSearch',
                     {
                         method: 'post',
                         body: JSON.stringify(textquery),
                         headers: new Headers({
                             'Accept': 'application/json',
                             'Content-Type': 'application/json'
                         })
               })
                .then(response => response.json() as Promise<IEcosiaResult>)
                .then(ecosiaResult => {
                    console.log('data returned', ecosiaResult);
                    this.ecosiaResults.push(ecosiaResult);
                    sumBingMacths = parseInt(this.ecosiaResultsSum().toString(), 0) + JSON.parse(ko.toJSON(ecosiaResult)).counter;;
                    this.ecosiaResultsSum(sumBingMacths);
                });
            

        }

       
    }

    
}

export default { viewModel: SearchMachineViewModel, template: require('./search-machine.html') };
