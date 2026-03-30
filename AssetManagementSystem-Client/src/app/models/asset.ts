export interface SoftwareLicense {
    licenseName: string;
}

export interface Asset {
    assetId: string;
    name: string;
    serialNumber: string;
    statusId: number;
    employeeName?: string;
    warrantyName?: string;
    softwareLicenses?: SoftwareLicense[];
}

export interface AssigneeAsset{
    assetId: string;
    employeeId: string;
}

export interface AddedAsset{
    name: string;
    serialNumber: string;
    warrantyName: string;
}

export interface EditAsset{
    name: string;
    serialNumber?: string;
    warrantyName?: string;
    employeeName?: string;
}
