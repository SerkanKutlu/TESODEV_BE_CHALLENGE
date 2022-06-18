package types

type Address struct {
	AddressLine string `bson:"addressLine" json:"addressLine"`
	City        string `bson:"city" json:"city"`
	Country     string `bson:"country" json:"country"`
	CityCode    int    `bson:"cityCode" json:"cityCode"`
}
