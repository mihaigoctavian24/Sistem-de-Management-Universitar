#!/bin/bash

# Credentials
SUPABASE_URL="https://wcmliinmbntmesfzxfmx.supabase.co"
ANON_KEY="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6IndjbWxpaW5tYm50bWVzZnp4Zm14Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjM2OTIyMDIsImV4cCI6MjA3OTI2ODIwMn0.XO6R5htHxN2TEnfOZddrYeqdJ_ANaItr57vDcoC2iLY"

# Function to create user
create_user() {
    EMAIL=$1
    PASSWORD=$2
    FIRST_NAME=$3
    LAST_NAME=$4
    ROLE=$5

    echo "Creating user: $EMAIL ($ROLE)..."

    curl -X POST "$SUPABASE_URL/auth/v1/signup" \
        -H "apikey: $ANON_KEY" \
        -H "Content-Type: application/json" \
        -d "{
            \"email\": \"$EMAIL\",
            \"password\": \"$PASSWORD\",
            \"data\": {
                \"first_name\": \"$FIRST_NAME\",
                \"last_name\": \"$LAST_NAME\",
                \"role\": \"$ROLE\"
            }
        }"
    echo ""
}

# Create Users
create_user "student.test@stud.rau.ro" "F32e891_!!_Admin" "Test" "Student" "student"
create_user "professor.test@stud.rau.ro" "F32e891_!!_Admin" "Test" "Professor" "professor"
create_user "dean.test@stud.rau.ro" "F32e891_!!_Admin" "Test" "Dean" "dean"
create_user "secretary.test@stud.rau.ro" "F32e891_!!_Admin" "Test" "Secretary" "secretary"
create_user "rector.test@stud.rau.ro" "F32e891_!!_Admin" "Test" "Rector" "rector"
