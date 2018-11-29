--
-- PostgreSQL database dump
--

-- Dumped from database version 9.3.4
-- Dumped by pg_dump version 9.3.4
-- Started on 2014-07-07 07:29:59

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 185 (class 3079 OID 11750)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2028 (class 0 OID 0)
-- Dependencies: 185
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 182 (class 1259 OID 49154)
-- Name: profiles; Type: TABLE; Schema: public; Owner: lgame; Tablespace: 
--

CREATE TABLE profiles (
    id integer NOT NULL,
    name character varying NOT NULL,
    pic_url character varying,
    pass character varying,
    "time" integer DEFAULT 0 NOT NULL,
    is_photo boolean NOT NULL
);


ALTER TABLE public.profiles OWNER TO lgame;

--
-- TOC entry 184 (class 1259 OID 49173)
-- Name: profiles_english_weather; Type: TABLE; Schema: public; Owner: lgame; Tablespace: 
--

CREATE TABLE profiles_english_weather (
    pid integer NOT NULL,
    lev_no integer NOT NULL,
    best_time integer DEFAULT 0 NOT NULL,
    fake_id integer NOT NULL
);


ALTER TABLE public.profiles_english_weather OWNER TO lgame;

--
-- TOC entry 183 (class 1259 OID 49171)
-- Name: profiles_english_weather_fake_id_seq; Type: SEQUENCE; Schema: public; Owner: lgame
--

CREATE SEQUENCE profiles_english_weather_fake_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.profiles_english_weather_fake_id_seq OWNER TO lgame;

--
-- TOC entry 2029 (class 0 OID 0)
-- Dependencies: 183
-- Name: profiles_english_weather_fake_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: lgame
--

ALTER SEQUENCE profiles_english_weather_fake_id_seq OWNED BY profiles_english_weather.fake_id;


--
-- TOC entry 181 (class 1259 OID 49152)
-- Name: profiles_id_seq; Type: SEQUENCE; Schema: public; Owner: lgame
--

CREATE SEQUENCE profiles_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.profiles_id_seq OWNER TO lgame;

--
-- TOC entry 2030 (class 0 OID 0)
-- Dependencies: 181
-- Name: profiles_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: lgame
--

ALTER SEQUENCE profiles_id_seq OWNED BY profiles.id;


--
-- TOC entry 171 (class 1259 OID 24581)
-- Name: themes; Type: TABLE; Schema: public; Owner: lgame; Tablespace: 
--

CREATE TABLE themes (
    id integer NOT NULL,
    "Name" character varying,
    "Width" integer,
    "Height" integer,
    "Left" integer,
    "Top" integer,
    "Data" character varying
);


ALTER TABLE public.themes OWNER TO lgame;

--
-- TOC entry 172 (class 1259 OID 32768)
-- Name: themes_english_letters; Type: TABLE; Schema: public; Owner: lgame; Tablespace: 
--

CREATE TABLE themes_english_letters (
    tid integer,
    letter "char",
    "left" integer,
    top integer,
    rotation integer,
    id integer NOT NULL
);


ALTER TABLE public.themes_english_letters OWNER TO lgame;

--
-- TOC entry 177 (class 1259 OID 32813)
-- Name: themes_english_letters_id_seq; Type: SEQUENCE; Schema: public; Owner: lgame
--

CREATE SEQUENCE themes_english_letters_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.themes_english_letters_id_seq OWNER TO lgame;

--
-- TOC entry 2031 (class 0 OID 0)
-- Dependencies: 177
-- Name: themes_english_letters_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: lgame
--

ALTER SEQUENCE themes_english_letters_id_seq OWNED BY themes_english_letters.id;


--
-- TOC entry 175 (class 1259 OID 32792)
-- Name: themes_english_pictures; Type: TABLE; Schema: public; Owner: lgame; Tablespace: 
--

CREATE TABLE themes_english_pictures (
    tid integer,
    wid integer,
    "left" integer,
    top integer,
    rotation integer,
    id integer NOT NULL
);


ALTER TABLE public.themes_english_pictures OWNER TO lgame;

--
-- TOC entry 176 (class 1259 OID 32805)
-- Name: themes_english_pictures_id_seq; Type: SEQUENCE; Schema: public; Owner: lgame
--

CREATE SEQUENCE themes_english_pictures_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.themes_english_pictures_id_seq OWNER TO lgame;

--
-- TOC entry 2032 (class 0 OID 0)
-- Dependencies: 176
-- Name: themes_english_pictures_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: lgame
--

ALTER SEQUENCE themes_english_pictures_id_seq OWNED BY themes_english_pictures.id;


--
-- TOC entry 170 (class 1259 OID 24579)
-- Name: themes_id_seq; Type: SEQUENCE; Schema: public; Owner: lgame
--

CREATE SEQUENCE themes_id_seq
    START WITH 1
    INCREMENT BY 1
    MINVALUE 0
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.themes_id_seq OWNER TO lgame;

--
-- TOC entry 2033 (class 0 OID 0)
-- Dependencies: 170
-- Name: themes_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: lgame
--

ALTER SEQUENCE themes_id_seq OWNED BY themes.id;


--
-- TOC entry 174 (class 1259 OID 32778)
-- Name: words; Type: TABLE; Schema: public; Owner: lgame; Tablespace: 
--

CREATE TABLE words (
    id integer,
    tid integer,
    pic_url character varying,
    back_clr character(6),
    border_clr character(6),
    left_clr character(6),
    top_clr character(6),
    left_top_clr character(6),
    fakeid integer NOT NULL
);


ALTER TABLE public.words OWNER TO lgame;

--
-- TOC entry 179 (class 1259 OID 40970)
-- Name: words_english; Type: TABLE; Schema: public; Owner: lgame; Tablespace: 
--

CREATE TABLE words_english (
    wid integer NOT NULL,
    value character varying,
    audio_url character varying,
    fakeid integer NOT NULL
);


ALTER TABLE public.words_english OWNER TO lgame;

--
-- TOC entry 180 (class 1259 OID 40976)
-- Name: words_english_fakeid_seq; Type: SEQUENCE; Schema: public; Owner: lgame
--

CREATE SEQUENCE words_english_fakeid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.words_english_fakeid_seq OWNER TO lgame;

--
-- TOC entry 2034 (class 0 OID 0)
-- Dependencies: 180
-- Name: words_english_fakeid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: lgame
--

ALTER SEQUENCE words_english_fakeid_seq OWNED BY words_english.fakeid;


--
-- TOC entry 178 (class 1259 OID 32821)
-- Name: words_fakeid_seq; Type: SEQUENCE; Schema: public; Owner: lgame
--

CREATE SEQUENCE words_fakeid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.words_fakeid_seq OWNER TO lgame;

--
-- TOC entry 2035 (class 0 OID 0)
-- Dependencies: 178
-- Name: words_fakeid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: lgame
--

ALTER SEQUENCE words_fakeid_seq OWNED BY words.fakeid;


--
-- TOC entry 173 (class 1259 OID 32776)
-- Name: words_id_seq; Type: SEQUENCE; Schema: public; Owner: lgame
--

CREATE SEQUENCE words_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.words_id_seq OWNER TO lgame;

--
-- TOC entry 2036 (class 0 OID 0)
-- Dependencies: 173
-- Name: words_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: lgame
--

ALTER SEQUENCE words_id_seq OWNED BY words.id;


--
-- TOC entry 1870 (class 2604 OID 49157)
-- Name: id; Type: DEFAULT; Schema: public; Owner: lgame
--

ALTER TABLE ONLY profiles ALTER COLUMN id SET DEFAULT nextval('profiles_id_seq'::regclass);


--
-- TOC entry 1873 (class 2604 OID 49177)
-- Name: fake_id; Type: DEFAULT; Schema: public; Owner: lgame
--

ALTER TABLE ONLY profiles_english_weather ALTER COLUMN fake_id SET DEFAULT nextval('profiles_english_weather_fake_id_seq'::regclass);


--
-- TOC entry 1865 (class 2604 OID 24584)
-- Name: id; Type: DEFAULT; Schema: public; Owner: lgame
--

ALTER TABLE ONLY themes ALTER COLUMN id SET DEFAULT nextval('themes_id_seq'::regclass);


--
-- TOC entry 1866 (class 2604 OID 32815)
-- Name: id; Type: DEFAULT; Schema: public; Owner: lgame
--

ALTER TABLE ONLY themes_english_letters ALTER COLUMN id SET DEFAULT nextval('themes_english_letters_id_seq'::regclass);


--
-- TOC entry 1868 (class 2604 OID 32807)
-- Name: id; Type: DEFAULT; Schema: public; Owner: lgame
--

ALTER TABLE ONLY themes_english_pictures ALTER COLUMN id SET DEFAULT nextval('themes_english_pictures_id_seq'::regclass);


--
-- TOC entry 1867 (class 2604 OID 32823)
-- Name: fakeid; Type: DEFAULT; Schema: public; Owner: lgame
--

ALTER TABLE ONLY words ALTER COLUMN fakeid SET DEFAULT nextval('words_fakeid_seq'::regclass);


--
-- TOC entry 1869 (class 2604 OID 40978)
-- Name: fakeid; Type: DEFAULT; Schema: public; Owner: lgame
--

ALTER TABLE ONLY words_english ALTER COLUMN fakeid SET DEFAULT nextval('words_english_fakeid_seq'::regclass);


--
-- TOC entry 2018 (class 0 OID 49154)
-- Dependencies: 182
-- Data for Name: profiles; Type: TABLE DATA; Schema: public; Owner: lgame
--

COPY profiles (id, name, pic_url, pass, "time", is_photo) FROM stdin;
1	net_yura	dd2c3385-90f5-46ff-af2e-d20aa9689f4e.jpg	7895	0	t
\.


--
-- TOC entry 2020 (class 0 OID 49173)
-- Dependencies: 184
-- Data for Name: profiles_english_weather; Type: TABLE DATA; Schema: public; Owner: lgame
--

COPY profiles_english_weather (pid, lev_no, best_time, fake_id) FROM stdin;
\.


--
-- TOC entry 2037 (class 0 OID 0)
-- Dependencies: 183
-- Name: profiles_english_weather_fake_id_seq; Type: SEQUENCE SET; Schema: public; Owner: lgame
--

SELECT pg_catalog.setval('profiles_english_weather_fake_id_seq', 1, false);


--
-- TOC entry 2038 (class 0 OID 0)
-- Dependencies: 181
-- Name: profiles_id_seq; Type: SEQUENCE SET; Schema: public; Owner: lgame
--

SELECT pg_catalog.setval('profiles_id_seq', 1, true);


--
-- TOC entry 2007 (class 0 OID 24581)
-- Dependencies: 171
-- Data for Name: themes; Type: TABLE DATA; Schema: public; Owner: lgame
--

COPY themes (id, "Name", "Width", "Height", "Left", "Top", "Data") FROM stdin;
1	Weather	78	78	86	79	M293,248 C301,219 322,197 322,197 L372,193 L397,210 L463,181 L507,125 L521,81 L581,87 L590,157 L579,204 L519,252 L496,278 L464,312 C464,312 400,331 394,331 C388,331 330,310 330,310 L300,276 z
2	Body	94	74	131	109	M467,312 L520,251 L574,210 L651,175 L775,187 L789,222 L807,236 L821,276 L812,369 L753,409 L572,384 L496,348 L471,327 z
3	plant1	103	79	161	39	M489,326 L492,347 L489,362 L506,354 L539,357 L558,340 L578,329 L590,316 L588,303 L579,294 L570,285 L552,291 L539,308 L516,312 z
4	plant2	105	81	139	0	M488,325 L469,322 L467,306 L474,287 L498,268 L517,270 L531,268 L555,248 L565,246 L557,271 L557,274 L557,275 L558,277 L559,278 L562,280 L564,281 L566,280 L568,282 L570,283 L553,289 L549,292 L546,295 L544,299 L539,304 L538,306 L516,310 L489,324 z
6	City	98	57	0	122	M492,311 L485,303 L481,297 L476,293 L475,289 L474,281 L476,275 L481,270 L486,266 L496,264 L518,267 L531,266 L552,258 L560,258 L558,265 L560,274 L563,278 L567,284 L570,285 L561,295 L554,294 L548,295 L546,297 L542,301 L539,303 L533,303 L501,313 z
\.


--
-- TOC entry 2008 (class 0 OID 32768)
-- Dependencies: 172
-- Data for Name: themes_english_letters; Type: TABLE DATA; Schema: public; Owner: lgame
--

COPY themes_english_letters (tid, letter, "left", top, rotation, id) FROM stdin;
1	W	13	40	4	30
1	E	27	41	-11	31
1	A	35	38	-27	32
1	T	43	33	-46	33
1	H	49	25	-53	34
1	E	55	16	-58	35
1	R	59	9	-58	36
2	B	32	21	-3	37
2	O	43	20	-3	38
2	D	56	20	-3	39
2	Y	69	19	-3	40
3	P	9	43	-18	41
3	L	20	40	-22	42
3	A	31	34	-21	43
3	N	45	28	-17	44
3	T	58	23	-29	45
3	(	67	16	-29	46
3	1	70	15	-29	47
3	)	74	12	-32	48
4	P	5	53	-39	49
4	L	15	46	-30	50
4	A	24	39	-27	51
4	N	36	34	-24	52
4	T	49	29	-17	53
4	(	58	26	-15	54
4	2	63	26	-16	55
4	)	70	23	-15	56
6	C	19	14	4	57
6	I	33	15	0	58
6	T	45	15	0	59
6	Y	57	14	-5	60
\.


--
-- TOC entry 2039 (class 0 OID 0)
-- Dependencies: 177
-- Name: themes_english_letters_id_seq; Type: SEQUENCE SET; Schema: public; Owner: lgame
--

SELECT pg_catalog.setval('themes_english_letters_id_seq', 60, true);


--
-- TOC entry 2011 (class 0 OID 32792)
-- Dependencies: 175
-- Data for Name: themes_english_pictures; Type: TABLE DATA; Schema: public; Owner: lgame
--

COPY themes_english_pictures (tid, wid, "left", top, rotation, id) FROM stdin;
1	1	49	39	-46	1
1	2	11	50	17	2
1	3	31	53	-23	3
2	64	14	36	12	4
2	63	40	39	-2	5
2	66	66	35	-24	6
3	29	17	52	-6	7
3	26	41	46	-20	8
3	36	62	36	-26	9
4	53	12	62	-26	10
4	49	35	49	-20	11
4	50	58	42	-30	12
6	100	14	30	7	13
6	106	38	30	0	14
6	111	63	25	-19	15
\.


--
-- TOC entry 2040 (class 0 OID 0)
-- Dependencies: 176
-- Name: themes_english_pictures_id_seq; Type: SEQUENCE SET; Schema: public; Owner: lgame
--

SELECT pg_catalog.setval('themes_english_pictures_id_seq', 15, true);


--
-- TOC entry 2041 (class 0 OID 0)
-- Dependencies: 170
-- Name: themes_id_seq; Type: SEQUENCE SET; Schema: public; Owner: lgame
--

SELECT pg_catalog.setval('themes_id_seq', 6, true);


--
-- TOC entry 2010 (class 0 OID 32778)
-- Dependencies: 174
-- Data for Name: words; Type: TABLE DATA; Schema: public; Owner: lgame
--

COPY words (id, tid, pic_url, back_clr, border_clr, left_clr, top_clr, left_top_clr, fakeid) FROM stdin;
2	1	dew.jpg	79D67A	84D685	8AB48A	9BB49C	9BD69C	82
3	1	fog.jpg	9796AD	9998AD	9B9AA5	9F9EA5	9F9EAD	83
104	6	snackbar.jpg	AD6F15	AD7628	A67A30	9E864E	AD864E	165
105	6	cinema.jpg	81392E	814238	7C463D	77544D	81544D	166
106	6	monument.jpg	8AA4D7	93AAD7	98ADBF	A6B7BF	A6B7D7	167
4	1	cold.jpg	F7F9FF	D2D2FF	D5D5EC	DFDFEC	DFDFFF	84
5	1	frost.jpg	95BFF8	A1C6F8	A7C9DB	BAD4DB	BAD4F8	85
6	1	hot.jpg	FF6E34	FF804D	F48859	E7A480	FFA480	86
7	1	hurricane.jpg	8C8A85	8C8A85	8C8A86	8C8A87	8C8A87	87
8	1	ice.jpg	D7DDE2	CECEE2	D0D0DA	D4D4DA	D4D4E2	88
9	1	icicle.jpg	A9C6E6	B0CAE6	B4CBD5	BFD2D5	BFD2E6	89
10	1	lightning.jpg	151747	1B1D47	1E1F35	272935	272947	90
11	1	puddle.jpg	777F7A	787F7A	787D7A	7A7D7B	7A7F7B	91
12	1	rain.jpg	406E9F	4B749F	517784	638084	63809F	92
13	1	rainbow.jpg	406EAE	4D76AE	54798E	69868E	6986AE	93
14	1	snow.jpg	B8CFEA	BECFEA	C1D1DB	CAD7DB	CAD7EA	94
58	1	overcast.jpg	323C3C	333C3C	333C3B	353C3B	353C3C	95
59	1	sunny.jpg	3296F0	49A1F0	54A6BC	79B7BC	79B7F0	96
60	1	windy.jpg	0096FF	1FA3FF	2EA9BC	5FBDBC	5FBDFF	97
61	1	hail.jpg	AFAFAF	AFAFAF	AFAFAF	AFAFAF	AFAFAF	98
62	1	tornado.jpg	28325A	2E375A	31394A	3A414A	3A415A	99
70	1	muddy.jpg	87918C	88918C	888F8C	8A8F8D	8A918D	100
15	2	ankle.jpg	D8B554	D8B964	D3BB6C	CDC285	D8C285	101
16	2	arch.jpg	F1CEBE	F1D0C4	EFD2C7	EBD9D1	F1D9D1	102
63	2	head.jpg	D29B8C	D2A194	CEA598	CAAFA6	D2AFA6	103
64	2	eye.jpg	FF1D65	FF3978	F34681	E5719E	FF719E	104
65	2	nose.jpg	BE9178	BE9680	BB9984	B7A192	BEA192	105
66	2	lips.jpg	FA573D	FA6B54	EF745F	E29483	FA9483	106
67	2	ear.jpg	AFAFAF	AFAFAF	AFAFAF	AFAFAF	AFAFAF	107
73	2	finger.jpg	D7AA8C	D7AF95	D4B299	CFBAA8	D7BAA8	108
74	2	stomach.jpg	A53C1E	A5492E	9E4F36	956350	A56350	109
75	2	legs.jpg	F0CDB9	F0D0BF	EDD2C3	EAD9CD	F0D9CD	110
76	2	calf.jpg	B9875F	B98D6A	B5906F	B09980	B99980	111
81	2	breast.jpg	C89164	C89770	C39B76	BEA589	C8A589	112
82	2	chin.jpg	C8A07D	C8A586	C5A78A	C1AF99	C8AF99	113
77	2	heel.jpg	C8876E	C88F79	C4927E	BE9F8F	C89F8F	114
78	2	toes.jpg	C88C6E	C89379	C4977E	BEA28F	C8A28F	115
79	2	body.jpg	969695	969695	969695	969695	969695	116
80	2	shin.jpg	D7A582	D7AB8C	D3AE91	CEB7A1	D7B7A1	117
83	2	hair.jpg	724B19	724F24	6E5229	6A593A	72593A	118
84	2	hand.jpg	ACA073	ACA17A	AAA27D	A8A488	ACA488	119
85	2	neck.jpg	F4BD82	F4C390	EFC796	E9D1AC	F4D1AC	120
17	3	bush.jpg	1FAE7B	30AE81	398A84	548A8E	54AE8E	121
18	3	tree.jpg	1D8040	298048	2F624B	426258	428058	122
19	3	root.jpg	463200	463408	44350C	40391A	46391A	123
20	3	branch.jpg	7BB3F2	89BAF2	90BED0	A7CAD0	A7CAF2	124
21	3	leaf.jpg	92C250	98C25E	9AA464	A4A47A	A4C27A	125
22	3	bud.jpg	43A51B	4FA52C	557934	67794E	67A54E	126
23	3	stub.jpg	6F867C	71867D	73807D	77807F	77867F	127
24	3	hollow.jpg	9BC36E	A0C378	A2AC7D	AAAC8D	AAC38D	128
25	3	bark.jpg	8C7964	8C7B69	8B7C6B	888073	8C8073	129
26	3	flower.jpg	FF46B4	FF5DBD	F768C1	ED8BD0	FF8BD0	130
27	3	apple.jpg	37BE1E	47BE32	4F873B	69875A	69BE5A	131
28	3	pear.jpg	CD5A3C	CD684E	C56F56	BB8572	CD8572	132
29	3	cherry.jpg	B91414	B92424	AE2E2E	A24E4E	B94E4E	133
30	3	peach.jpg	B9370F	B9471F	AF4E2A	A4674B	B9674B	134
31	3	strawberry.jpg	C3F140	C32529	B73034	AB5255	C35255	135
32	3	raspberry.jpg	FFFFFF	E1293F	D4354A	C65D6D	E15D6D	136
33	3	pineapple.jpg	A18714	A18A25	9C8B2D	969048	A19048	137
34	3	grape.jpg	FFFFFF	B92424	AE2E2E	A24E4E	B94E4E	138
35	3	acorn.jpg	64A050	6BA018	6F7821	7A783F	7AA03F	139
36	3	nut.jpg	AF8714	AF8C27	A98E30	A2964E	AF964E	140
37	4	cone.jpg	946B4D	947055	91725A	8D7A67	947A67	141
38	4	pine.jpg	B8C7B0	B9C7B2	BAC0B4	BDC0B8	BDC7B8	142
39	4	birch.jpg	D7D7D7	CDCDD7	CECED3	D0D0D3	D0D0D7	143
40	4	grass.jpg	50BEA0	5DBE20	64882B	79884D	79BE4D	144
41	4	hay.jpg	8C8741	8C874A	8A874E	87885D	8C885D	145
42	4	dandelion.jpg	2874CC	2884DC	348AA3	5B9DA3	5B9DDC	146
43	4	sunflower.jpg	2866DA	2876FA	367FB1	649CB1	649CFA	147
44	4	potato.jpg	C8B978	C8BA82	C5BB86	C2BE96	C8BE96	148
107	6	church.jpg	E6A1BB	E6A9BD	E3ADC0	DFBAC9	E6BAC9	168
45	4	cabbage.jpg	78913C	7B9146	7C7D4B	817D5B	81915B	149
46	4	onion.jpg	DC731E	DC8035	D38640	C89A65	DC9A65	150
47	4	garlic.jpg	C8C391	C8C397	C7C39B	C5C4A5	C8C4A5	151
48	4	beet.jpg	C83741	C84951	BF5159	B56D73	C86D73	152
49	4	carrot.jpg	DC4B19	DC5D31	D1653C	C58162	DC8162	153
50	4	cucumber.jpg	1E9B0A	2D9B1C	346924	4C6940	4C9B40	154
51	4	tomato.jpg	D1321E	D14534	C64F3E	BA6D61	D16D61	155
52	4	melon.jpg	E6AA0A	E6B125	DDB532	D3C05C	E6C05C	156
53	4	watermelon.jpg	73872D	758738	76733D	7A734E	7A874E	157
54	4	pea.jpg	46AF1E	53AF30	598138	6D8154	6DAF54	158
55	4	wheat.jpg	F0AD00	F0B51E	E6B92C	DBC65A	F0C65A	159
56	4	buckwheat.jpg	90511E	90582C	8B5C32	846848	906848	160
100	6	city.jpg	847CEA	9089EA	9690C3	AAA5C3	AAA5EA	161
101	6	shop.jpg	C50F0D	C52524	B9302E	AC5352	C55352	162
102	6	library.jpg	9B4600	9B5013	94551C	8B653A	9B653A	163
103	6	hospital.jpg	99A5BA	9DA7BA	9FA8B0	A5ACB0	A5ACBA	164
108	6	bridge.jpg	4987AC	558BAC	5B8D93	6E9493	6E94AC	169
109	6	gate.jpg	88889F	8A8A9F	8C8C97	909097	90909F	170
110	6	road.jpg	644B3C	644E41	634F43	60544B	64544B	171
111	6	trafficlights.jpg	5087C8	5F8FC8	6692A6	7D9FA6	7D9FC8	172
112	6	pavement.jpg	7D7D8C	7E7E8C	7F7F87	828287	82828C	173
113	6	bench.jpg	DEC435	DEC74A	D8C854	D1CD74	DECD74	174
114	6	car.jpg	4B5F9B	55669B	596A81	697581	69759B	175
115	6	lorry.jpg	9B9691	9B9692	9B9692	9B9794	9B9794	176
116	6	bus.jpg	415564	455664	47575B	4E5A5B	4E5A64	177
117	6	fillingstation.jpg	F05861	F06B72	E7737B	DC9196	F09196	178
118	6	zebracrossing.jpg	818697	838897	858990	898C90	898C97	179
119	6	intersection.jpg	8C93A2	8E94A2	90959C	94989C	9498A2	180
1	1	cloud.jpg	347FFF	4D8FFF	5996C2	80AFC2	80AFFF	81
\.


--
-- TOC entry 2015 (class 0 OID 40970)
-- Dependencies: 179
-- Data for Name: words_english; Type: TABLE DATA; Schema: public; Owner: lgame
--

COPY words_english (wid, value, audio_url, fakeid) FROM stdin;
1	cloud	cloud.mp3	1
2	dew	dew.mp3	2
3	fog	fog.mp3	3
4	cold	cold.mp3	4
5	frost	frost.mp3	5
6	hot	hot.mp3	6
7	hurricane	hurricane.mp3	7
8	ice	ice.mp3	8
9	icicle	icicle.mp3	9
10	lightning	lightning.mp3	10
11	puddle	puddle.mp3	11
12	rain	rain.mp3	12
13	rainbow	rainbow.mp3	13
14	snow	snow.mp3	14
58	overcast	overcast.mp3	15
59	sunny	sunny.mp3	16
60	windy	windy.mp3	17
61	hail	hail.mp3	18
62	tornado	tornado.mp3	19
70	muddy	muddy.mp3	20
15	ankle	ankle.mp3	21
16	arch	arch.mp3	22
63	head	head.mp3	23
64	eye	eye.mp3	24
65	nose	nose.mp3	25
66	lips	lips.mp3	26
67	ear	ear.mp3	27
73	finger	finger.mp3	28
74	stomach	stomach.mp3	29
75	legs	legs.mp3	30
76	calf	calf.mp3	31
81	breast	breast.mp3	32
82	chin	chin.mp3	33
77	heel	heel.mp3	34
78	toes	toes.mp3	35
79	body	body.mp3	36
80	shin	shin.mp3	37
83	hair	hair.mp3	38
84	hand	hand.mp3	39
85	neck	neck.mp3	40
17	bush	bush.mp3	41
18	tree	tree.mp3	42
19	root	root.mp3	43
20	branch	branch.mp3	44
21	leaf	leaf.mp3	45
22	bud	bud.mp3	46
23	stub	stub.mp3	47
24	hollow	hollow.mp3	48
25	bark	bark.mp3	49
26	flower	flower.mp3	50
27	apple	apple.mp3	51
28	pear	pear.mp3	52
29	cherry	cherry.mp3	53
30	peach	peach.mp3	54
31	strawberry	strawberry.mp3	55
32	raspberry	raspberry.mp3	56
33	pineapple	pineapple.mp3	57
34	grape	grape.mp3	58
35	acorn	acorn.mp3	59
36	nut	nut.mp3	60
37	cone	cone.mp3	61
38	pine	pine.mp3	62
39	birch	birch.mp3	63
40	grass	grass.mp3	64
41	hay	hay.mp3	65
42	dandelion	dandelion.mp3	66
43	sunflower	sunflower.mp3	67
44	potato	potato.mp3	68
45	cabbage	cabbage.mp3	69
46	onion	onion.mp3	70
47	garlic	garlic.mp3	71
48	beet	beet.mp3	72
49	carrot	carrot.mp3	73
50	cucumber	cucumber.mp3	74
51	tomato	tomato.mp3	75
52	melon	melon.mp3	76
53	watermelon	watermelon.mp3	77
54	pea	pea.mp3	78
55	wheat	wheat.mp3	79
56	buckwheat	buckwheat.mp3	80
100	city	city.mp3	81
101	shop	shop.mp3	82
102	library	library.mp3	83
103	hospital	hospital.mp3	84
104	snackbar	snackbar.mp3	85
105	cinema	cinema.mp3	86
106	monument	monument.mp3	87
107	church	church.mp3	88
108	bridge	bridge.mp3	89
109	gate	gate.mp3	90
110	road	road.mp3	91
111	traffic lights	trafficlights.mp3	92
112	pavement	pavement.mp3	93
113	bench	bench.mp3	94
114	car	car.mp3	95
115	lorry	lorry.mp3	96
116	bus	bus.mp3	97
117	filling station	fillingstation.mp3	98
118	zebra crossing	zebracrossing.mp3	99
119	intersection	intersection.mp3	100
\.


--
-- TOC entry 2042 (class 0 OID 0)
-- Dependencies: 180
-- Name: words_english_fakeid_seq; Type: SEQUENCE SET; Schema: public; Owner: lgame
--

SELECT pg_catalog.setval('words_english_fakeid_seq', 100, true);


--
-- TOC entry 2043 (class 0 OID 0)
-- Dependencies: 178
-- Name: words_fakeid_seq; Type: SEQUENCE SET; Schema: public; Owner: lgame
--

SELECT pg_catalog.setval('words_fakeid_seq', 180, true);


--
-- TOC entry 2044 (class 0 OID 0)
-- Dependencies: 173
-- Name: words_id_seq; Type: SEQUENCE SET; Schema: public; Owner: lgame
--

SELECT pg_catalog.setval('words_id_seq', 1, false);


--
-- TOC entry 1894 (class 2606 OID 49179)
-- Name: profiles_english_weather_fakeid_pk; Type: CONSTRAINT; Schema: public; Owner: lgame; Tablespace: 
--

ALTER TABLE ONLY profiles_english_weather
    ADD CONSTRAINT profiles_english_weather_fakeid_pk PRIMARY KEY (fake_id);


--
-- TOC entry 1892 (class 2606 OID 49163)
-- Name: profiles_id_pk; Type: CONSTRAINT; Schema: public; Owner: lgame; Tablespace: 
--

ALTER TABLE ONLY profiles
    ADD CONSTRAINT profiles_id_pk PRIMARY KEY (id);


--
-- TOC entry 1877 (class 2606 OID 32820)
-- Name: themes_english_letters_id_pk; Type: CONSTRAINT; Schema: public; Owner: lgame; Tablespace: 
--

ALTER TABLE ONLY themes_english_letters
    ADD CONSTRAINT themes_english_letters_id_pk PRIMARY KEY (id);


--
-- TOC entry 1885 (class 2606 OID 32812)
-- Name: themes_english_pictures_id_pk; Type: CONSTRAINT; Schema: public; Owner: lgame; Tablespace: 
--

ALTER TABLE ONLY themes_english_pictures
    ADD CONSTRAINT themes_english_pictures_id_pk PRIMARY KEY (id);


--
-- TOC entry 1875 (class 2606 OID 24589)
-- Name: themes_id_pk; Type: CONSTRAINT; Schema: public; Owner: lgame; Tablespace: 
--

ALTER TABLE ONLY themes
    ADD CONSTRAINT themes_id_pk PRIMARY KEY (id);


--
-- TOC entry 1887 (class 2606 OID 40986)
-- Name: words_english_fakeid_pk; Type: CONSTRAINT; Schema: public; Owner: lgame; Tablespace: 
--

ALTER TABLE ONLY words_english
    ADD CONSTRAINT words_english_fakeid_pk PRIMARY KEY (fakeid);


--
-- TOC entry 1890 (class 2606 OID 40988)
-- Name: words_english_wid_uk; Type: CONSTRAINT; Schema: public; Owner: lgame; Tablespace: 
--

ALTER TABLE ONLY words_english
    ADD CONSTRAINT words_english_wid_uk UNIQUE (wid);


--
-- TOC entry 1879 (class 2606 OID 32831)
-- Name: words_fakeid_pk; Type: CONSTRAINT; Schema: public; Owner: lgame; Tablespace: 
--

ALTER TABLE ONLY words
    ADD CONSTRAINT words_fakeid_pk PRIMARY KEY (fakeid);


--
-- TOC entry 1881 (class 2606 OID 32833)
-- Name: words_id_uk; Type: CONSTRAINT; Schema: public; Owner: lgame; Tablespace: 
--

ALTER TABLE ONLY words
    ADD CONSTRAINT words_id_uk UNIQUE (id);


--
-- TOC entry 1883 (class 1259 OID 32839)
-- Name: fki_themes_english_pictures_wid_fk; Type: INDEX; Schema: public; Owner: lgame; Tablespace: 
--

CREATE INDEX fki_themes_english_pictures_wid_fk ON themes_english_pictures USING btree (wid);


--
-- TOC entry 1888 (class 1259 OID 40989)
-- Name: words_english_wid_index; Type: INDEX; Schema: public; Owner: lgame; Tablespace: 
--

CREATE INDEX words_english_wid_index ON words_english USING btree (wid);


--
-- TOC entry 1882 (class 1259 OID 40969)
-- Name: words_tid_index; Type: INDEX; Schema: public; Owner: lgame; Tablespace: 
--

CREATE INDEX words_tid_index ON words USING btree (tid);


--
-- TOC entry 1895 (class 2606 OID 32771)
-- Name: themes_english_letters_tid_fk; Type: FK CONSTRAINT; Schema: public; Owner: lgame
--

ALTER TABLE ONLY themes_english_letters
    ADD CONSTRAINT themes_english_letters_tid_fk FOREIGN KEY (tid) REFERENCES themes(id);


--
-- TOC entry 1897 (class 2606 OID 32795)
-- Name: themes_english_pictures_tid_fk; Type: FK CONSTRAINT; Schema: public; Owner: lgame
--

ALTER TABLE ONLY themes_english_pictures
    ADD CONSTRAINT themes_english_pictures_tid_fk FOREIGN KEY (tid) REFERENCES themes(id);


--
-- TOC entry 1898 (class 2606 OID 32834)
-- Name: themes_english_pictures_wid_fk; Type: FK CONSTRAINT; Schema: public; Owner: lgame
--

ALTER TABLE ONLY themes_english_pictures
    ADD CONSTRAINT themes_english_pictures_wid_fk FOREIGN KEY (wid) REFERENCES words(id);


--
-- TOC entry 1896 (class 2606 OID 32785)
-- Name: words_tid_fk; Type: FK CONSTRAINT; Schema: public; Owner: lgame
--

ALTER TABLE ONLY words
    ADD CONSTRAINT words_tid_fk FOREIGN KEY (tid) REFERENCES themes(id);


--
-- TOC entry 2027 (class 0 OID 0)
-- Dependencies: 5
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2014-07-07 07:30:00

--
-- PostgreSQL database dump complete
--

