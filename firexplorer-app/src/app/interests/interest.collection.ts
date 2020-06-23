import { IPublicationSet } from './interest.publication.set';

export interface IInterestCollection{
    identifier: string,
    name : string,
    currentValue : number,
    historicalValue : number,
    publicationSets : IPublicationSet[],
    gitHubPathHash : string
}