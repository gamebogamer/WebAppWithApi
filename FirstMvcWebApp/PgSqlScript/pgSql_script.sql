DO $$
BEGIN
    IF NOT EXISTS (
        SELECT FROM information_schema.tables 
        WHERE table_name = 't_users'
    ) THEN
        CREATE TABLE t_users (
            c_userid SERIAL PRIMARY KEY,
            c_username VARCHAR(50) NOT NULL,
            c_email VARCHAR(100) NOT NULL UNIQUE,
            c_password VARCHAR(255) NOT NULL,
            c_dateofbirth DATE NOT NULL,
            c_gender VARCHAR(10) NOT NULL,
            c_phonenumber VARCHAR(20) NOT NULL UNIQUE,
            c_address VARCHAR(255) NOT NULL,
            c_usertype VARCHAR(20) NOT NULL,
            c_status VARCHAR(10) NOT NULL DEFAULT 'active',
            c_createdat TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
            c_updatedat TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
            c_hobby VARCHAR(100)
        );
    END IF;
END $$;