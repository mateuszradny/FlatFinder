export enum OfferStatus {
    New,
    Active,
    Removed
}

export class PurchaseOffer {
    offerId: number;
    userEmail: string;
    userPhone: string;
    flatId: number;
    flatName: string;
    status: OfferStatus;
}