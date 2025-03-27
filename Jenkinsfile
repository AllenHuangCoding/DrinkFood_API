pipeline {
    agent any
    
    stages {
        stage('Checkout') {
            steps {
                echo 'Fetching source code...'
                checkout scm
            }
        }
        
        stage('Build') {
            steps {
                echo 'Building the application...'
                sh 'echo "Simulating build process..."'
            }
        }
        
        stage('Test') {
            steps {
                echo 'Running tests...'
                sh 'echo "Simulating tests..."'
            }
        }
        
        stage('Deploy') {
            steps {
                echo 'Deploying the application...'
                sh 'echo "Simulating deployment..."'
            }
        }
    }
    
    post {
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed! Check logs for details.'
        }
    }
}
