-- Table: public.t_users

DROP TABLE IF EXISTS public.t_users;

CREATE TABLE public.t_users (
    c_userid SERIAL PRIMARY KEY,
    c_username TEXT NOT NULL,
    c_email TEXT NOT NULL UNIQUE,
    c_password TEXT NOT NULL,
    c_dateofbirth TIMESTAMPTZ NOT NULL,
    c_gender TEXT NOT NULL,
    c_phonenumber TEXT NOT NULL UNIQUE,
    c_address TEXT NOT NULL,
    c_usertype TEXT NOT NULL,
    c_status TEXT NOT NULL,
    c_createdat TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP NOT NULL,
    c_updatedat TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    c_hobby TEXT NOT NULL
);

ALTER TABLE public.t_users
    OWNER TO postgres;

-- Table: public.t_activetokens

DROP TABLE IF EXISTS public.t_activetokens;

CREATE TABLE public.t_activetokens (
    c_activetokenid SERIAL PRIMARY KEY,
    c_userid INTEGER NOT NULL,
    c_usertoken TEXT NOT NULL,
    c_tokenissuedatutc TIMESTAMPTZ NOT NULL,
    c_tokenexpiryatutc TIMESTAMPTZ NOT NULL,
    CONSTRAINT fk_t_activetokens_t_users FOREIGN KEY (c_userid)
        REFERENCES public.t_users (c_userid) ON DELETE CASCADE
);

ALTER TABLE public.t_activetokens
    OWNER TO postgres;

-- Indexes for t_activetokens
CREATE INDEX IF NOT EXISTS idx_t_activetokens_c_userid
    ON public.t_activetokens (c_userid);
