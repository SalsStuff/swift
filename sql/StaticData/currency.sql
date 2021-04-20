CREATE TABLE "currency" (
	"index_id" BIGINT NOT NULL IDENTITY	PRIMARY KEY,
	"currency_code" VARCHAR(50) NULL,
	"currency_name" VARCHAR(100) NULL,
	"entity" VARCHAR(100) NULL,
); 
create unique index currency_idx on currency (index_id);

BEGIN TRANSACTION T01
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AED', 'UAE Dirham',														'UNITED ARAB EMIRATES (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AFN', 'Afghani',															'AFGHANISTAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ALL', 'Lek',																'ALBANIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AMD', 'Armenian Dram',														'ARMENIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ANG', 'Netherlands Antillean Guilder',										'CURAÇAO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ANG', 'Netherlands Antillean Guilder',										'SINT MAARTEN (DUTCH PART)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AOA', 'Kwanza',															'ANGOLA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ARS', 'Argentine Peso',													'ARGENTINA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AUD', 'Australian Dollar',													'AUSTRALIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AUD', 'Australian Dollar',													'CHRISTMAS ISLAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AUD', 'Australian Dollar',													'COCOS (KEELING) ISLANDS (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AUD', 'Australian Dollar',													'HEARD ISLAND AND McDONALD ISLANDS');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AUD', 'Australian Dollar',													'KIRIBATI');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AUD', 'Australian Dollar',													'NAURU');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AUD', 'Australian Dollar',													'NORFOLK ISLAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AUD', 'Australian Dollar',													'TUVALU');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AWG', 'Aruban Florin',														'ARUBA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('AZN', 'Azerbaijan Manat',													'AZERBAIJAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BAM', 'Convertible Mark',													'BOSNIA AND HERZEGOVINA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BBD', 'Barbados Dollar',													'BARBADOS');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BDT', 'Taka',																'BANGLADESH');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BGN', 'Bulgarian Lev',														'BULGARIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BHD', 'Bahraini Dinar',													'BAHRAIN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BIF', 'Burundi Franc',														'BURUNDI');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BMD', 'Bermudian Dollar',													'BERMUDA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BND', 'Brunei Dollar',														'BRUNEI DARUSSALAM');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BOB', 'Boliviano',															'BOLIVIA (PLURINATIONAL STATE OF)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BOV', 'Mvdol',																'BOLIVIA (PLURINATIONAL STATE OF)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BRL', 'Brazilian Real',													'BRAZIL');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BSD', 'Bahamian Dollar',													'BAHAMAS (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BTN', 'Ngultrum',															'BHUTAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BWP', 'Pula',																'BOTSWANA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BYN', 'Belarusian Ruble',													'BELARUS');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('BZD', 'Belize Dollar',														'BELIZE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CAD', 'Canadian Dollar',													'CANADA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CDF', 'Congolese Franc',													'CONGO (THE DEMOCRATIC REPUBLIC OF THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CHE', 'WIR Euro',															'SWITZERLAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CHF', 'Swiss Franc',														'LIECHTENSTEIN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CHF', 'Swiss Franc',														'SWITZERLAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CHW', 'WIR Franc',															'SWITZERLAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CLF', 'Unidad de Fomento',													'CHILE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CLP', 'Chilean Peso',														'CHILE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CNY', 'Yuan Renminbi',														'CHINA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('COP', 'Colombian Peso',													'COLOMBIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('COU', 'Unidad de Valor Real',												'COLOMBIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CRC', 'Costa Rican Colon',													'COSTA RICA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CUC', 'Peso Convertible',													'CUBA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CUP', 'Cuban Peso',														'CUBA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CVE', 'Cabo Verde Escudo',													'CABO VERDE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('CZK', 'Czech Koruna',														'CZECHIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('DJF', 'Djibouti Franc',													'DJIBOUTI');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('DKK', 'Danish Krone',														'DENMARK');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('DKK', 'Danish Krone',														'FAROE ISLANDS (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('DKK', 'Danish Krone',														'GREENLAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('DOP', 'Dominican Peso',													'DOMINICAN REPUBLIC (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('DZD', 'Algerian Dinar',													'ALGERIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EGP', 'Egyptian Pound',													'EGYPT');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ERN', 'Nakfa',																'ERITREA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ETB', 'Ethiopian Birr',													'ETHIOPIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'ÅLAND ISLANDS');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'ANDORRA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'AUSTRIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'BELGIUM');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'CYPRUS');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'ESTONIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'EUROPEAN UNION');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'FINLAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'FRANCE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'FRENCH GUIANA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'FRENCH SOUTHERN TERRITORIES (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'GERMANY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'GREECE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'GUADELOUPE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'HOLY SEE (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'IRELAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'ITALY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'LATVIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'LITHUANIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'LUXEMBOURG');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'MALTA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'MARTINIQUE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'MAYOTTE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'MONACO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'MONTENEGRO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'NETHERLANDS (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'PORTUGAL');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'RÉUNION');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'SAINT BARTHÉLEMY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'SAINT MARTIN (FRENCH PART)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'SAINT PIERRE AND MIQUELON');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'SAN MARINO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'SLOVAKIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'SLOVENIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('EUR', 'Euro',																'SPAIN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('FJD', 'Fiji Dollar',														'FIJI');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('FKP', 'Falkland Islands Pound',											'FALKLAND ISLANDS (THE) [MALVINAS]');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('GBP', 'Pound Sterling',													'GUERNSEY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('GBP', 'Pound Sterling',													'ISLE OF MAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('GBP', 'Pound Sterling',													'JERSEY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('GBP', 'Pound Sterling',													'UNITED KINGDOM OF GREAT BRITAIN AND NORTHERN IRELAND (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('GEL', 'Lari',																'GEORGIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('GHS', 'Ghana Cedi',														'GHANA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('GIP', 'Gibraltar Pound',													'GIBRALTAR');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('GMD', 'Dalasi',															'GAMBIA (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('GNF', 'Guinean Franc',														'GUINEA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('GTQ', 'Quetzal',															'GUATEMALA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('GYD', 'Guyana Dollar',														'GUYANA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('HKD', 'Hong Kong Dollar',													'HONG KONG');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('HNL', 'Lempira',															'HONDURAS');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('HRK', 'Kuna',																'CROATIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('HTG', 'Gourde',															'HAITI');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('HUF', 'Forint',															'HUNGARY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('IDR', 'Rupiah',															'INDONESIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ILS', 'New Israeli Sheqel',												'ISRAEL');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('INR', 'Indian Rupee',														'BHUTAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('INR', 'Indian Rupee',														'INDIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('IQD', 'Iraqi Dinar',														'IRAQ');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('IRR', 'Iranian Rial',														'IRAN (ISLAMIC REPUBLIC OF)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ISK', 'Iceland Krona',														'ICELAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('JMD', 'Jamaican Dollar',													'JAMAICA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('JOD', 'Jordanian Dinar',													'JORDAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('JPY', 'Yen',																'JAPAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('KES', 'Kenyan Shilling',													'KENYA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('KGS', 'Som',																'KYRGYZSTAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('KHR', 'Riel',																'CAMBODIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('KMF', 'Comorian Franc',													'COMOROS (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('KPW', 'North Korean Won',													'KOREA (THE DEMOCRATIC PEOPLE’S REPUBLIC OF)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('KRW', 'Won',																'KOREA (THE REPUBLIC OF)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('KWD', 'Kuwaiti Dinar',														'KUWAIT');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('KYD', 'Cayman Islands Dollar',												'CAYMAN ISLANDS (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('KZT', 'Tenge',																'KAZAKHSTAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('LAK', 'Lao Kip',															'LAO PEOPLE’S DEMOCRATIC REPUBLIC (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('LBP', 'Lebanese Pound',													'LEBANON');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('LKR', 'Sri Lanka Rupee',													'SRI LANKA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('LRD', 'Liberian Dollar',													'LIBERIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('LSL', 'Loti',																'LESOTHO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('LYD', 'Libyan Dinar',														'LIBYA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MAD', 'Moroccan Dirham',													'MOROCCO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MAD', 'Moroccan Dirham',													'WESTERN SAHARA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MDL', 'Moldovan Leu',														'MOLDOVA (THE REPUBLIC OF)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MGA', 'Malagasy Ariary',													'MADAGASCAR');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MKD', 'Denar',																'NORTH MACEDONIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MMK', 'Kyat',																'MYANMAR');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MNT', 'Tugrik',															'MONGOLIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MOP', 'Pataca',															'MACAO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MRU', 'Ouguiya',															'MAURITANIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MUR', 'Mauritius Rupee',													'MAURITIUS');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MVR', 'Rufiyaa',															'MALDIVES');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MWK', 'Malawi Kwacha',														'MALAWI');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MXN', 'Mexican Peso',														'MEXICO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MXV', 'Mexican Unidad de Inversion (UDI)',									'MEXICO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MYR', 'Malaysian Ringgit',													'MALAYSIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('MZN', 'Mozambique Metical',												'MOZAMBIQUE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NAD', 'Namibia Dollar',													'NAMIBIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NGN', 'Naira',																'NIGERIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NIO', 'Cordoba Oro',														'NICARAGUA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NOK', 'Norwegian Krone',													'BOUVET ISLAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NOK', 'Norwegian Krone',													'NORWAY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NOK', 'Norwegian Krone',													'SVALBARD AND JAN MAYEN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NPR', 'Nepalese Rupee',													'NEPAL');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NZD', 'New Zealand Dollar',												'COOK ISLANDS (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NZD', 'New Zealand Dollar',												'NEW ZEALAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NZD', 'New Zealand Dollar',												'NIUE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NZD', 'New Zealand Dollar',												'PITCAIRN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('NZD', 'New Zealand Dollar',												'TOKELAU');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('OMR', 'Rial Omani',														'OMAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('PAB', 'Balboa',															'PANAMA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('PEN', 'Sol',																'PERU');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('PGK', 'Kina',																'PAPUA NEW GUINEA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('PHP', 'Philippine Peso',													'PHILIPPINES (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('PKR', 'Pakistan Rupee',													'PAKISTAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('PLN', 'Zloty',																'POLAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('PYG', 'Guarani',															'PARAGUAY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('QAR', 'Qatari Rial',														'QATAR');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('RON', 'Romanian Leu',														'ROMANIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('RSD', 'Serbian Dinar',														'SERBIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('RUB', 'Russian Ruble',														'RUSSIAN FEDERATION (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('RWF', 'Rwanda Franc',														'RWANDA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SAR', 'Saudi Riyal',														'SAUDI ARABIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SBD', 'Solomon Islands Dollar',											'SOLOMON ISLANDS');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SCR', 'Seychelles Rupee',													'SEYCHELLES');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SDG', 'Sudanese Pound',													'SUDAN (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SEK', 'Swedish Krona',														'SWEDEN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SGD', 'Singapore Dollar',													'SINGAPORE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SHP', 'Saint Helena Pound',												'SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SLL', 'Leone',																'SIERRA LEONE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SOS', 'Somali Shilling',													'SOMALIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SRD', 'Surinam Dollar',													'SURINAME');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SSP', 'South Sudanese Pound',												'SOUTH SUDAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('STN', 'Dobra',																'SAO TOME AND PRINCIPE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SVC', 'El Salvador Colon',													'EL SALVADOR');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SYP', 'Syrian Pound',														'SYRIAN ARAB REPUBLIC');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('SZL', 'Lilangeni',															'ESWATINI');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('THB', 'Baht',																'THAILAND');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('TJS', 'Somoni',															'TAJIKISTAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('TMT', 'Turkmenistan New Manat',											'TURKMENISTAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('TND', 'Tunisian Dinar',													'TUNISIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('TOP', 'Pa’anga',															'TONGA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('TRY', 'Turkish Lira',														'TURKEY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('TTD', 'Trinidad and Tobago Dollar',										'TRINIDAD AND TOBAGO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('TWD', 'New Taiwan Dollar',													'TAIWAN (PROVINCE OF CHINA)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('TZS', 'Tanzanian Shilling',												'TANZANIA, UNITED REPUBLIC OF');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('UAH', 'Hryvnia',															'UKRAINE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('UGX', 'Uganda Shilling',													'UGANDA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'AMERICAN SAMOA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'"BONAIRE, SINT EUSTATIUS AND SABA"');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'BRITISH INDIAN OCEAN TERRITORY (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'ECUADOR');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'EL SALVADOR');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'GUAM');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'HAITI');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'MARSHALL ISLANDS (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'MICRONESIA (FEDERATED STATES OF)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'NORTHERN MARIANA ISLANDS (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'PALAU');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'PANAMA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'PUERTO RICO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'TIMOR-LESTE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'TURKS AND CAICOS ISLANDS (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'UNITED STATES MINOR OUTLYING ISLANDS (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'UNITED STATES OF AMERICA (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'VIRGIN ISLANDS (BRITISH)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USD', 'US Dollar',															'VIRGIN ISLANDS (U.S.)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('USN', 'US Dollar (Next day)',												'UNITED STATES OF AMERICA (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('UYI', 'Uruguay Peso en Unidades Indexadas (UI)',							'URUGUAY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('UYU', 'Peso Uruguayo',														'URUGUAY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('UYW', 'Unidad Previsional',												'URUGUAY');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('UZS', 'Uzbekistan Sum',													'UZBEKISTAN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('VES', 'Bolívar Soberano',													'VENEZUELA (BOLIVARIAN REPUBLIC OF)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('VND', 'Dong',																'VIET NAM');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('VUV', 'Vatu',																'VANUATU');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('WST', 'Tala',																'SAMOA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XAF', 'CFA Franc BEAC',													'CAMEROON');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XAF', 'CFA Franc BEAC',													'CENTRAL AFRICAN REPUBLIC (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XAF', 'CFA Franc BEAC',													'CHAD');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XAF', 'CFA Franc BEAC',													'CONGO (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XAF', 'CFA Franc BEAC',													'EQUATORIAL GUINEA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XAF', 'CFA Franc BEAC',													'GABON');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XAG', 'Silver',															'ZZ11_Silver');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XAU', 'Gold',																'ZZ08_Gold');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XBA', 'Bond Markets Unit European Composite Unit (EURCO)',					'ZZ01_Bond Markets Unit European_EURCO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XBB', 'Bond Markets Unit European Monetary Unit (E.M.U.-6)',				'ZZ02_Bond Markets Unit European_EMU-6');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XBC', 'Bond Markets Unit European Unit of Account 9 (E.U.A.-9)',			'ZZ03_Bond Markets Unit European_EUA-9');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XBD', 'Bond Markets Unit European Unit of Account 17 (E.U.A.-17)',			'ZZ04_Bond Markets Unit European_EUA-17');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XCD', 'East Caribbean Dollar',												'ANGUILLA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XCD', 'East Caribbean Dollar',												'ANTIGUA AND BARBUDA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XCD', 'East Caribbean Dollar',												'DOMINICA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XCD', 'East Caribbean Dollar',												'GRENADA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XCD', 'East Caribbean Dollar',												'MONTSERRAT');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XCD', 'East Caribbean Dollar',												'SAINT KITTS AND NEVIS');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XCD', 'East Caribbean Dollar',												'SAINT LUCIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XCD', 'East Caribbean Dollar',												'SAINT VINCENT AND THE GRENADINES');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XDR', 'SDR (Special Drawing Right)',										'INTERNATIONAL MONETARY FUND (IMF)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XOF', 'CFA Franc BCEAO',													'BENIN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XOF', 'CFA Franc BCEAO',													'BURKINA FASO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XOF', 'CFA Franc BCEAO',													'CÔTE DIVOIRE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XOF', 'CFA Franc BCEAO',													'GUINEA-BISSAU');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XOF', 'CFA Franc BCEAO',													'MALI');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XOF', 'CFA Franc BCEAO',													'NIGER (THE)');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XOF', 'CFA Franc BCEAO',													'SENEGAL');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XOF', 'CFA Franc BCEAO',													'TOGO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XPD', 'Palladium',															'ZZ09_Palladium');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XPF', 'CFP Franc',															'FRENCH POLYNESIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XPF', 'CFP Franc',															'NEW CALEDONIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XPF', 'CFP Franc',															'WALLIS AND FUTUNA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XPT', 'Platinum',															'ZZ10_Platinum');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XSU', 'Sucre',																'SISTEMA UNITARIO DE COMPENSACION REGIONAL DE PAGOS SUCRE');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XTS', 'Codes specifically reserved for testing purposes',					'ZZ06_Testing_Code');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XUA', 'ADB Unit of Account',												'MEMBER COUNTRIES OF THE AFRICAN DEVELOPMENT BANK GROUP');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('XXX', 'The codes assigned for transactions where no currency is involved',	'ZZ07_No_Currency');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('YER', 'Yemeni Rial',														'YEMEN');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ZAR', 'Rand',																'LESOTHO');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ZAR', 'Rand',																'NAMIBIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ZAR', 'Rand',																'SOUTH AFRICA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ZMW', 'Zambian Kwacha',													'ZAMBIA');
INSERT INTO currency (currency_code, currency_name, entity) VALUES ('ZWL', 'Zimbabwe Dollar',													'ZIMBABWE');
COMMIT TRANSACTION T01																	