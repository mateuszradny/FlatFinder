import { Image } from './index';

export class Flat {
    id: number;

    address: string;
    area: string;
    city: string;
    createdOn: Date;
    description: string;
    floor: number;
    hasBalcony: boolean;
    modifiedOn: Date;
    name: string;
    numberOfRooms: number;
    postCode: string;
    price: number;
    isSold: boolean;

    ownerId: number;
    images: Image[];
}