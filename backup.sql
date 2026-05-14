--
-- PostgreSQL database dump
--

\restrict bAZocfbtezDSo0ZIALmv1Z9VWLllp4qndXOgTnJVNyIgTcMnXklHpgYe0kt3Okw

-- Dumped from database version 15.17
-- Dumped by pg_dump version 15.17

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: maintenance_records; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.maintenance_records (
    note_id integer NOT NULL,
    machine_id integer,
    maintenance_date date NOT NULL,
    description text,
    problems text,
    done_by_user integer
);


ALTER TABLE public.maintenance_records OWNER TO postgres;

--
-- Name: maintenance_records_note_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.maintenance_records_note_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.maintenance_records_note_id_seq OWNER TO postgres;

--
-- Name: maintenance_records_note_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.maintenance_records_note_id_seq OWNED BY public.maintenance_records.note_id;


--
-- Name: products; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.products (
    product_id integer NOT NULL,
    name character varying(255) NOT NULL,
    description text,
    price numeric(10,2),
    in_stock integer,
    min_stock integer,
    propensity_to_sell numeric(5,2),
    CONSTRAINT chk_price_positive CHECK ((price > (0)::numeric)),
    CONSTRAINT chk_stock_non_negative CHECK (((in_stock >= 0) AND (min_stock >= 0)))
);


ALTER TABLE public.products OWNER TO postgres;

--
-- Name: products_product_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.products_product_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.products_product_id_seq OWNER TO postgres;

--
-- Name: products_product_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.products_product_id_seq OWNED BY public.products.product_id;


--
-- Name: sales; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sales (
    sale_id integer NOT NULL,
    machine_id integer,
    product_id integer,
    quantity integer,
    sale_sum numeric(10,2),
    sale_datetime timestamp without time zone NOT NULL,
    payment_type character varying(50),
    CONSTRAINT chk_quantity_positive CHECK ((quantity > 0)),
    CONSTRAINT chk_sale_payment_type CHECK (((payment_type)::text = ANY ((ARRAY['Карта'::character varying, 'Наличные'::character varying, 'QR-код'::character varying])::text[]))),
    CONSTRAINT chk_sale_sum_non_negative CHECK ((sale_sum >= (0)::numeric))
);


ALTER TABLE public.sales OWNER TO postgres;

--
-- Name: sales_sale_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.sales_sale_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.sales_sale_id_seq OWNER TO postgres;

--
-- Name: sales_sale_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.sales_sale_id_seq OWNED BY public.sales.sale_id;


--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    userid integer NOT NULL,
    fullname character varying(255) NOT NULL,
    email character varying(255),
    contacts character varying(255),
    role character varying(50) DEFAULT 'Оператор'::character varying,
    password_hash character varying(255),
    CONSTRAINT chk_user_role CHECK (((role)::text = ANY ((ARRAY['Администратор'::character varying, 'Оператор'::character varying])::text[])))
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: users_userid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_userid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.users_userid_seq OWNER TO postgres;

--
-- Name: users_userid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_userid_seq OWNED BY public.users.userid;


--
-- Name: vending_machines; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.vending_machines (
    machine_id integer NOT NULL,
    location character varying(500) NOT NULL,
    model character varying(100) NOT NULL,
    payment_type character varying(50),
    full_income numeric(15,2) DEFAULT 0,
    serial_number character varying(100) NOT NULL,
    inventory_number character varying(100) NOT NULL,
    manufacturer character varying(200),
    manufacture_date date NOT NULL,
    date_of_commissioning date NOT NULL,
    last_verification_date date,
    verification_interval integer,
    resource_hours integer,
    date_of_next_fixing date,
    maintenance_time_hours integer,
    machine_status character varying(50),
    country character varying(100),
    inventory_date date,
    last_checked_by_user character varying(255),
    CONSTRAINT chk_commissioning_date CHECK ((date_of_commissioning >= manufacture_date)),
    CONSTRAINT chk_machine_status CHECK (((machine_status)::text = ANY ((ARRAY['Работает'::character varying, 'Вышел из строя'::character varying, 'В ремонте/на обслуживании'::character varying])::text[]))),
    CONSTRAINT chk_maintenance_time_range CHECK (((maintenance_time_hours >= 1) AND (maintenance_time_hours <= 20))),
    CONSTRAINT chk_payment_type CHECK (((payment_type)::text = ANY ((ARRAY['с оплатой картой'::character varying, 'с оплатой наличными'::character varying, 'два вида оплаты'::character varying])::text[]))),
    CONSTRAINT chk_resource_hours_positive CHECK ((resource_hours > 0))
);


ALTER TABLE public.vending_machines OWNER TO postgres;

--
-- Name: vending_machines_machine_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.vending_machines_machine_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.vending_machines_machine_id_seq OWNER TO postgres;

--
-- Name: vending_machines_machine_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.vending_machines_machine_id_seq OWNED BY public.vending_machines.machine_id;


--
-- Name: maintenance_records note_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.maintenance_records ALTER COLUMN note_id SET DEFAULT nextval('public.maintenance_records_note_id_seq'::regclass);


--
-- Name: products product_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products ALTER COLUMN product_id SET DEFAULT nextval('public.products_product_id_seq'::regclass);


--
-- Name: sales sale_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales ALTER COLUMN sale_id SET DEFAULT nextval('public.sales_sale_id_seq'::regclass);


--
-- Name: users userid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN userid SET DEFAULT nextval('public.users_userid_seq'::regclass);


--
-- Name: vending_machines machine_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vending_machines ALTER COLUMN machine_id SET DEFAULT nextval('public.vending_machines_machine_id_seq'::regclass);


--
-- Data for Name: maintenance_records; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.maintenance_records (note_id, machine_id, maintenance_date, description, problems, done_by_user) FROM stdin;
1	3	2026-01-22	Плановое ТО: очистка камер, проверка датчиков, смазка механизмов	Загрязнение датчиков наличия товара, ложные срабатывания	1
2	2	2026-01-21	Пополнение запасов: загружены 50 шт. воды, 30 шт. снеков	Низкий уровень запасов: осталось 5 бутылок воды, 2 батончика	2
3	1	2026-01-20	Замена вышедшего из строя дисплея управления	Экран не реагирует на касания, вероятный обрыв шлейфа	3
4	7	2026-01-19	Чистка системы подачи напитков, промывка трубок	Протечка в системе подачи воды, износ уплотнителя	4
5	6	2026-01-18	Обновление ПО до версии 2.1.5, перезагрузка системы	Ошибка связи с платёжным терминалом (код 105)	5
6	5	2026-01-17	Регулировка механизма выдачи товара, калибровка сенсоров	Заедание механизма выдачи, скопление мусора в лотке	6
7	9	2026-01-16	Замена аккумулятора резервного питания	Разряд резервного аккумулятора ниже 20 %	7
8	10	2026-01-15	Пополнение монетного механизма, инкассация наличных	Некорректное отображение цен на экране (сбой кэша)	8
9	8	2026-01-14	Установка нового модуля безналичной оплаты	Повреждение кабеля питания, оголение контактов	9
10	4	2026-01-13	Проверка герметичности корпуса, устранение зазоров	Повышенный шум вентилятора охлаждения, износ подшипников	10
\.


--
-- Data for Name: products; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.products (product_id, name, description, price, in_stock, min_stock, propensity_to_sell) FROM stdin;
1	Кофе «Эспрессо»	Эспрессо из 100 % арабики, без добавок. Объём: 250 мл	120.00	18	5	3.50
2	Чипсы «Сыр & Лук»	Картофельные чипсы с ароматом сыра и лука. Без ГМО	95.00	25	8	2.10
3	Вода минеральная негазированная	Природная минеральная вода, низкоминерализованная. Без газа	60.00	40	10	4.80
4	Шоколадный батончик «Ореховый восторг»	Молочный шоколад с цельным фундуком и карамельной начинкой	85.00	30	7	1.90
5	Газированный напиток «Кола»	Газированный напиток со вкусом колы, с кофеином	75.00	22	6	2.70
6	Смесь орехов «Классика»	Смесь миндаля, фундука и грецкого ореха, слегка подсоленная	150.00	15	4	1.20
7	Леденцы «Мятные»	Мятные леденцы без сахара, с натуральным ароматизатором	45.00	50	12	5.30
8	Попкорн «Сливочный»	Воздушный попкорн со сливочным маслом и солью	70.00	28	9	1.80
9	Энергетический напиток «Turbo»	Энергетический напиток с таурином, кофеином и витаминами группы B	130.00	12	3	2.40
\.


--
-- Data for Name: sales; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sales (sale_id, machine_id, product_id, quantity, sale_sum, sale_datetime, payment_type) FROM stdin;
1	2	9	1	120.00	2026-01-22 08:15:30	Карта
2	5	2	3	285.00	2026-01-22 10:45:12	Наличные
3	9	6	2	120.00	2026-01-22 12:30:45	QR-код
4	8	5	1	85.00	2026-01-22 14:20:05	Карта
5	6	7	4	300.00	2026-01-22 16:55:22	Наличные
6	1	1	1	150.00	2026-01-22 18:03:17	QR-код
7	3	4	5	225.00	2026-01-22 19:40:50	Карта
8	10	2	2	140.00	2026-01-22 21:10:33	Наличные
9	4	8	1	130.00	2026-01-22 22:50:47	QR-код
10	7	7	3	165.00	2026-01-22 23:59:01	Карта
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (userid, fullname, email, contacts, role, password_hash) FROM stdin;
1	Иванов Алексей Петрович	alex.ivanov@example.com	+7 916 123‑45‑67	Администратор	$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy
2	Петрова Мария Ивановна	maria.petrova@mail.ru	+7 903 234‑56‑78	Оператор	$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy
3	Сидоров Дмитрий Викторович	dmitry.sidorov@yandex.ru	+7 926 345‑67‑89	Оператор	$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy
4	Кузнецова Елена Павловна	elena.kuznetsova@gmail.com	+7 915 456‑78‑90	Оператор	$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy
5	Морозов Роман Николаевич	roman.morozov@company.org	+7 909 567‑89‑01	Администратор	$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy
6	Волкова Татьяна Леонидовна	tatyana.volkova@example.net	+7 925 678‑90‑12	Оператор	$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy
7	Алексеев Сергей Михайлович	sergey.alekseev@biz.ru	+7 910 789‑01‑23	Оператор	$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy
8	Никитина Ольга Александровна	olga.nikitina@proton.me	+7 905 890‑12‑34	Оператор	$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy
9	Фёдоров Игорь Борисович	igor.fedorov@outlook.com	+7 927 901‑23‑45	Администратор	$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy
10	Григорьева Наталья Константиновна	natalia.grigorieva@mail.com	+7 901 012‑34‑56	Администратор	$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy
11	Антон Чугур	anton.chugur@mail.ru	+79424242424	Оператор	$2a$11$XIcrALVcRgeUa5mC0J1iJelISs5WolFIXRSU.eKnlJcG9OgoChmN2
\.


--
-- Data for Name: vending_machines; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.vending_machines (machine_id, location, model, payment_type, full_income, serial_number, inventory_number, manufacturer, manufacture_date, date_of_commissioning, last_verification_date, verification_interval, resource_hours, date_of_next_fixing, maintenance_time_hours, machine_status, country, inventory_date, last_checked_by_user) FROM stdin;
1	г. Санкт‑Петербург, Невский пр., д. 50, ТЦ «Галерея», 2‑й этаж.	VendCore X‑200	с оплатой картой	1250000.00	SC123456789	INV‑2025‑001	ООО «ВендТех»	2025-05-01	2025-05-10	2025-06-15	6	2500	2026-08-01	4	Работает	Россия	2025-07-20	Иванов А. С.
2	Московская обл., г. Химки, ул. Московская, д. 15, офис 301.	CoffeeMaster Pro 500	с оплатой наличными	1250000.00	SN987654321	INV‑2025‑002	АО «КофеМаш»	2025-06-15	2025-06-20	2025-07-25	12	1800	2026-09-10	8	Вышел из строя	Китай	2025-08-15	Петрова М. И.
3	г. Казань, ул. Баумана, д. 20, кафетерий	SnackVend S‑300	два вида оплаты	1250000.00	VCX200‑001	INV‑2025‑003	ЗАО «СнекВенд»	2025-07-20	2025-07-22	2025-08-05	24	1801	2026-10-20	12	В ремонте/на обслуживании	Германия	2025-09-10	Сидоров Д. В.
4	г. Екатеринбург, ул. Ленина, д. 50, холл бизнес‑центра.	AquaVend Water 2025	с оплатой картой	1250000.00	CM500‑PRO‑002	INV‑2025‑004	ООО «АкваВенд»	2025-08-10	2025-08-15	2025-09-20	18	1802	2026-11-05	6	Работает	Южная Корея	2025-09-10	Кузнецова Е. П.
5	г. Новосибирск, Красный пр., д. 100, университетский кампус.	VendoTech Elite 400	два вида оплаты	1250000.00	SV300‑SN003	INV‑2025‑005	ООО «ТехноВенд»	2025-09-25	2025-09-30	2025-10-01	36	1803	2026-12-15	16	В ремонте/на обслуживании	США	2025-11-10	Морозов Р. Н.
6	г. Сочи, Курортный пр., д. 70, отель «Морская звезда», лобби.	QuickBite Mini 100	с оплатой наличными	1250000.00	AW2025‑004	INV‑2025‑006	ИП «МиниВенд»	2025-10-05	2025-10-10	2025-11-15	12	1804	2026-03-25	10	Вышел из строя	Италия	2025-12-10	Волкова Т. Л.
7	г. Нижний Новгород, ул. Большая Покровская, д. 40, торговый пассаж.	HotDrink Station 600	с оплатой картой	1250000.00	VT400‑ELT‑005	INV‑2025‑007	ООО «ГорячийНапиток»	2025-11-12	2025-11-15	2025-12-22	6	1805	2026-03-03	3	Работает	Турция	2025-12-10	Алексеев С. М.
8	г. Самара, ул. Молодогвардейская, д. 120, ТЦ «Мега».	FreshFood Vend 700	два вида оплаты	1250000.00	QB100‑MIN‑006	INV‑2025‑008	АО «ФрэшФудВенд»	2025-12-18	2025-12-20	2026-01-05	24	1806	2026-04-12	18	В ремонте/на обслуживании	Япония	2026-01-10	Никитина О. А.
9	г. Ростов‑на‑Дону, ул. Садовая, д. 80, административное здание.	IceCream Vend 250	с оплатой наличными	1250000.00	HDS600‑007	INV‑2025‑009	ООО «АйсВенд»	2026-01-03	2026-01-10	2026-01-12	18	1807	2026-05-22	7	Вышел из строя	Польша	2026-01-13	Фёдоров И. Б.
10	г. Владивосток, ул. Светланская, д. 60, морской вокзал.	Print&Go Kiosk 150	с оплатой картой	1250000.00	FF700‑VND‑008	INV‑2025‑010	ЗАО «ПринтВенд»	2026-01-14	2026-01-20	2026-01-25	12	1808	2026-07-02	14	Работает	Тайвань	2026-01-22	Григорьева Н. К.
\.


--
-- Name: maintenance_records_note_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.maintenance_records_note_id_seq', 1, false);


--
-- Name: products_product_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.products_product_id_seq', 1, false);


--
-- Name: sales_sale_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sales_sale_id_seq', 1, false);


--
-- Name: users_userid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_userid_seq', 11, true);


--
-- Name: vending_machines_machine_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.vending_machines_machine_id_seq', 1, false);


--
-- Name: maintenance_records maintenance_records_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.maintenance_records
    ADD CONSTRAINT maintenance_records_pkey PRIMARY KEY (note_id);


--
-- Name: products products_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT products_pkey PRIMARY KEY (product_id);


--
-- Name: sales sales_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales
    ADD CONSTRAINT sales_pkey PRIMARY KEY (sale_id);


--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (userid);


--
-- Name: vending_machines vending_machines_inventory_number_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vending_machines
    ADD CONSTRAINT vending_machines_inventory_number_key UNIQUE (inventory_number);


--
-- Name: vending_machines vending_machines_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vending_machines
    ADD CONSTRAINT vending_machines_pkey PRIMARY KEY (machine_id);


--
-- Name: vending_machines vending_machines_serial_number_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vending_machines
    ADD CONSTRAINT vending_machines_serial_number_key UNIQUE (serial_number);


--
-- Name: maintenance_records maintenance_records_done_by_user_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.maintenance_records
    ADD CONSTRAINT maintenance_records_done_by_user_fkey FOREIGN KEY (done_by_user) REFERENCES public.users(userid);


--
-- Name: maintenance_records maintenance_records_machine_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.maintenance_records
    ADD CONSTRAINT maintenance_records_machine_id_fkey FOREIGN KEY (machine_id) REFERENCES public.vending_machines(machine_id);


--
-- Name: sales sales_machine_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales
    ADD CONSTRAINT sales_machine_id_fkey FOREIGN KEY (machine_id) REFERENCES public.vending_machines(machine_id);


--
-- Name: sales sales_product_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales
    ADD CONSTRAINT sales_product_id_fkey FOREIGN KEY (product_id) REFERENCES public.products(product_id);


--
-- PostgreSQL database dump complete
--

\unrestrict bAZocfbtezDSo0ZIALmv1Z9VWLllp4qndXOgTnJVNyIgTcMnXklHpgYe0kt3Okw

