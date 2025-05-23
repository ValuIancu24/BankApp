--
-- PostgreSQL database dump
--

-- Dumped from database version 17.5
-- Dumped by pg_dump version 17.5

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
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
-- Name: Accounts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Accounts" (
    "Id" integer NOT NULL,
    "AccountNumber" character varying(20) NOT NULL,
    "Balance" numeric(18,2) NOT NULL,
    "Currency" character varying(3) NOT NULL,
    "SpendingLimit" numeric(18,2) NOT NULL,
    "DailyWithdrawalLimit" numeric(18,2) NOT NULL,
    "Status" character varying(20) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "UserId" integer NOT NULL
);


ALTER TABLE public."Accounts" OWNER TO postgres;

--
-- Name: Accounts_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Accounts" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Accounts_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: BillPayments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."BillPayments" (
    "Id" integer NOT NULL,
    "BillType" character varying(50) NOT NULL,
    "Provider" character varying(50) NOT NULL,
    "BillNumber" character varying(50) NOT NULL,
    "Amount" numeric NOT NULL,
    "Currency" character varying(3) NOT NULL,
    "Note" character varying(255),
    "Status" character varying(20) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "TransactionId" integer NOT NULL,
    "AccountId" integer NOT NULL
);


ALTER TABLE public."BillPayments" OWNER TO postgres;

--
-- Name: BillPayments_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."BillPayments" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."BillPayments_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Notifications; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Notifications" (
    "Id" integer NOT NULL,
    "Title" character varying(100) NOT NULL,
    "Message" character varying(255) NOT NULL,
    "Type" character varying(20) NOT NULL,
    "IsRead" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UserId" integer NOT NULL,
    "TransactionId" integer
);


ALTER TABLE public."Notifications" OWNER TO postgres;

--
-- Name: Notifications_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Notifications" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Notifications_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Transactions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Transactions" (
    "Id" integer NOT NULL,
    "Amount" numeric(18,2) NOT NULL,
    "Currency" character varying(3) NOT NULL,
    "Type" character varying(20) NOT NULL,
    "Note" character varying(255),
    "Status" character varying(20) NOT NULL,
    "IsImportant" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "FromAccountId" integer NOT NULL,
    "ToAccountId" integer,
    "ToAccountNumber" character varying(50),
    "FromAccountNumber" character varying(50)
);


ALTER TABLE public."Transactions" OWNER TO postgres;

--
-- Name: Transactions_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Transactions" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Transactions_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" integer NOT NULL,
    "Username" character varying(50) NOT NULL,
    "PasswordHash" text NOT NULL,
    "FirstName" character varying(50) NOT NULL,
    "LastName" character varying(50) NOT NULL,
    "Email" character varying(100) NOT NULL,
    "CNP" character varying(13) NOT NULL,
    "City" character varying(50) NOT NULL,
    "BirthDate" timestamp with time zone NOT NULL,
    "PhoneNumber" character varying(15) NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- Name: Users_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Users" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Users_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: Accounts; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Accounts" ("Id", "AccountNumber", "Balance", "Currency", "SpendingLimit", "DailyWithdrawalLimit", "Status", "CreatedAt", "UpdatedAt", "UserId") FROM stdin;
1	RO7547219495569476	0.00	RON	5000.00	2000.00	Active	2025-05-14 20:05:13.950227+03	2025-05-14 20:06:49.539672+03	3
2	RO0838219784272459	800.00	USD	5000.00	2000.00	Active	2025-05-14 20:05:38.51404+03	2025-05-14 21:53:19.254036+03	3
3	RO4455132338950172	1680.00	EUR	50000.00	2000.00	Active	2025-05-14 20:05:46.189609+03	2025-05-14 21:53:19.254057+03	3
\.


--
-- Data for Name: BillPayments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."BillPayments" ("Id", "BillType", "Provider", "BillNumber", "Amount", "Currency", "Note", "Status", "CreatedAt", "TransactionId", "AccountId") FROM stdin;
\.


--
-- Data for Name: Notifications; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Notifications" ("Id", "Title", "Message", "Type", "IsRead", "CreatedAt", "UserId", "TransactionId") FROM stdin;
\.


--
-- Data for Name: Transactions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Transactions" ("Id", "Amount", "Currency", "Type", "Note", "Status", "IsImportant", "CreatedAt", "UpdatedAt", "FromAccountId", "ToAccountId", "ToAccountNumber", "FromAccountNumber") FROM stdin;
1	100.00	RON	Transfer		Completed	f	2025-05-14 20:06:49.539406+03	\N	1	3	RO4455132338950172	RO7547219495569476
2	200.00	USD	Transfer		Completed	f	2025-05-14 21:53:19.253814+03	\N	2	3	RO4455132338950172	RO0838219784272459
\.


--
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" ("Id", "Username", "PasswordHash", "FirstName", "LastName", "Email", "CNP", "City", "BirthDate", "PhoneNumber", "IsActive", "CreatedAt", "UpdatedAt") FROM stdin;
1	demo	588c55f3ce2b8569b153c5abbf13f9f74308b88a20017cc699b835cc93195d16	Demo	User	demo@example.com	1234567890123	Bucharest	1995-05-14 19:51:23.545456+03	0712345678	t	2025-05-14 19:51:23.545529+03	\N
2	admin	3eb3fe66b31e3b4d10fa70b5cad49c7112294af6ae4e476a1c405155d45aa121	Admin	User	admin@example.com	9876543210123	Craiova	1985-05-14 19:51:23.545596+03	0723456789	t	2025-05-14 19:51:23.545596+03	\N
3	Dobre29	421b7ccc708d56828f2864f53c4f373a6f378f4a69fdc6128233394958642dbb	Alex	Dobre	alex.dobre@gmail.com	5030822450047	Bucuresti	2002-02-02 02:00:00+02	0769696969	t	2025-05-14 20:03:11.264673+03	\N
4	Dariusg	a80b568a237f50391d2f1f97beaf99564e33d2e1c8a2e5cac21ceda701570312	Darius	Garland	dariusg@gmail.com	5020822450047	Braila	2001-11-09 02:00:00+02	0720422722	t	2025-05-14 21:55:09.545549+03	\N
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250514142027_InitialMigration	9.0.5
20250514164900_initmigr	9.0.5
\.


--
-- Name: Accounts_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Accounts_Id_seq"', 3, true);


--
-- Name: BillPayments_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."BillPayments_Id_seq"', 1, false);


--
-- Name: Notifications_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Notifications_Id_seq"', 1, false);


--
-- Name: Transactions_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Transactions_Id_seq"', 2, true);


--
-- Name: Users_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Users_Id_seq"', 4, true);


--
-- Name: Accounts PK_Accounts; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Accounts"
    ADD CONSTRAINT "PK_Accounts" PRIMARY KEY ("Id");


--
-- Name: BillPayments PK_BillPayments; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BillPayments"
    ADD CONSTRAINT "PK_BillPayments" PRIMARY KEY ("Id");


--
-- Name: Notifications PK_Notifications; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Notifications"
    ADD CONSTRAINT "PK_Notifications" PRIMARY KEY ("Id");


--
-- Name: Transactions PK_Transactions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Transactions"
    ADD CONSTRAINT "PK_Transactions" PRIMARY KEY ("Id");


--
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Accounts_AccountNumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Accounts_AccountNumber" ON public."Accounts" USING btree ("AccountNumber");


--
-- Name: IX_Accounts_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Accounts_UserId" ON public."Accounts" USING btree ("UserId");


--
-- Name: IX_BillPayments_AccountId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_BillPayments_AccountId" ON public."BillPayments" USING btree ("AccountId");


--
-- Name: IX_BillPayments_TransactionId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_BillPayments_TransactionId" ON public."BillPayments" USING btree ("TransactionId");


--
-- Name: IX_Notifications_TransactionId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Notifications_TransactionId" ON public."Notifications" USING btree ("TransactionId");


--
-- Name: IX_Notifications_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Notifications_UserId" ON public."Notifications" USING btree ("UserId");


--
-- Name: IX_Transactions_FromAccountId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Transactions_FromAccountId" ON public."Transactions" USING btree ("FromAccountId");


--
-- Name: IX_Transactions_ToAccountId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Transactions_ToAccountId" ON public."Transactions" USING btree ("ToAccountId");


--
-- Name: IX_Users_CNP; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Users_CNP" ON public."Users" USING btree ("CNP");


--
-- Name: IX_Users_Email; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Users_Email" ON public."Users" USING btree ("Email");


--
-- Name: IX_Users_Username; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Users_Username" ON public."Users" USING btree ("Username");


--
-- Name: Accounts FK_Accounts_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Accounts"
    ADD CONSTRAINT "FK_Accounts_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: BillPayments FK_BillPayments_Accounts_AccountId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BillPayments"
    ADD CONSTRAINT "FK_BillPayments_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES public."Accounts"("Id") ON DELETE CASCADE;


--
-- Name: BillPayments FK_BillPayments_Transactions_TransactionId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BillPayments"
    ADD CONSTRAINT "FK_BillPayments_Transactions_TransactionId" FOREIGN KEY ("TransactionId") REFERENCES public."Transactions"("Id") ON DELETE CASCADE;


--
-- Name: Notifications FK_Notifications_Transactions_TransactionId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Notifications"
    ADD CONSTRAINT "FK_Notifications_Transactions_TransactionId" FOREIGN KEY ("TransactionId") REFERENCES public."Transactions"("Id");


--
-- Name: Notifications FK_Notifications_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Notifications"
    ADD CONSTRAINT "FK_Notifications_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: Transactions FK_Transactions_Accounts_FromAccountId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Transactions"
    ADD CONSTRAINT "FK_Transactions_Accounts_FromAccountId" FOREIGN KEY ("FromAccountId") REFERENCES public."Accounts"("Id") ON DELETE RESTRICT;


--
-- Name: Transactions FK_Transactions_Accounts_ToAccountId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Transactions"
    ADD CONSTRAINT "FK_Transactions_Accounts_ToAccountId" FOREIGN KEY ("ToAccountId") REFERENCES public."Accounts"("Id") ON DELETE RESTRICT;


--
-- PostgreSQL database dump complete
--

