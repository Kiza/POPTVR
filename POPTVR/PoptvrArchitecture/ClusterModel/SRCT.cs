using System;

namespace POPTVR.PoptvrArchitecture.ClusterModel
{
    class SRCT : ClusterInterface
    {
        private const double LEARN_CONST = 0.01;
        private const double WIDTH_CONSTANT = 10.0;
        private const int MAXLOOPS = 1000;

        private int numberOfInputs;
        private int numberOfOutputs;
        private int numberOfClusters;

        private double [,,] clusters;
        private double [,,] clusterWidths;

        private double[,] synapticInputs;
        private double[,] synapticOutputs;

        private double [,] nodeProbabilities;

        private double [,] centroids;
        private double [,] widths;

        public double[,] Centroids
        {
            get
            {
                return this.centroids;
            }
        }

        public double[,] Widths
        {
            get
            {
                return this.widths;
            }
        }

        public SRCT(int numberOfInputs, int numberOfClusters, int numberOfOutputs)
        {       
            this.numberOfInputs = numberOfInputs;
	        this.numberOfOutputs = numberOfOutputs;
	        this.numberOfClusters = numberOfClusters;

	        // For each output node, create centroids for a set of labels
	        this.clusters = new double [this.numberOfOutputs, this.numberOfClusters, this.numberOfInputs];
	        
            // For each output node, create widths for a set of labels
	        this.clusterWidths = new double [this.numberOfOutputs, this.numberOfClusters, this.numberOfInputs];

	        for (int i=0; i< this.numberOfOutputs; i++) 
            {
		        for (int j=0; j< this.numberOfClusters;j++) 
                {
			        for (int k=0; k<this.numberOfInputs;k++) 
                    {
				        this.clusters[i,j,k] = 0.0;
			        }
		        }
	        }

            this.synapticInputs = new double[this.numberOfOutputs, this.numberOfClusters];
            this.synapticOutputs = new double[this.numberOfOutputs, this.numberOfClusters];
            this.nodeProbabilities = new double[this.numberOfOutputs, this.numberOfClusters];
        }

        public void computeInitialCentroids(double [,]inputdata, double [,]outputdata, int count) 
        {
	        // For each output node
	        for (int k=0; k<this.numberOfOutputs; k++)
            {
		        for (int i=0; i< this.numberOfInputs; i++)
                {
			        double max = Double.MinValue;
			        double min = Double.MaxValue;

			        // Transverse thru the whole pattern list to retrieve those belonging
			        // to input Xi.
			        for (int j=0; j < count; j++) 
                    {
                        if (max < inputdata[j, i] && outputdata[j, k] == 1.0)
                        {
                            max = inputdata[j, i];
                        }
                        if (min > inputdata[j, i] && outputdata[j, k] == 1.0)
                        {
                            min = inputdata[j, i];
                        }
			        }
			
			        // Then for each set of input pattern X for its corresponding output Y
			        // using equal partitions
			        double factor = (max-min)/(this.numberOfClusters + 1);
			        for (int j=1; j<= this.numberOfClusters; j++) 
                    {
				        this.clusters[k,j-1,i] = min + (factor * j);
			        }
		        }
	        }

            Console.WriteLine("Initial Clusters: ");
            for (int k = 0; k < this.numberOfOutputs; k++)
            {
                Console.WriteLine("For output node {0:D}", k);
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    Console.Write("For cluster {0:D}:\t", j);
                    for (int i = 0; i < this.numberOfInputs; i++)
                    {
                        Console.Write("{0:F3}\t", this.clusters[k, j, i]);
                    }
                    Console.WriteLine();
                }
            }
        }

        public void computeFinalCentroids(double [,]inputdata, double [,]outputdata, int count) 
        {
            double[,] euclideanDistance = new double[this.numberOfOutputs, this.numberOfClusters];
            double [] distVector = new double [this.numberOfInputs];

	        for (int loop = 1; loop <= MAXLOOPS; loop++) 
            {
		        for (int j=0; j<count; j++) 
                {
			        for (int i=0; i< this.numberOfOutputs; i++) 
                    {
				        for (int k=0; k < this.numberOfClusters; k++) 
                        {
					        euclideanDistance[i, k] = 0.0;
					        for (int l=0; l< this.numberOfInputs; l++) 
                            {
						        euclideanDistance[i, k] += (inputdata[j, l] - this.clusters[i, k, l])*(inputdata[j, l] - this.clusters[i, k, l]);
					        }
				        }
			        }

			        // For each output node, transverse through all the labels/clusters
			        int minOpIndex = 0;
                    int minIndex = 0;
			        for (int i=0; i< this.numberOfOutputs; i++) 
                    {
				        // based on supervised signal, find the label with the smallest
				        // euclidean distance
				        if (outputdata[j, i] == 1.0) 
                        {
					        minIndex = 0;
					        double min = euclideanDistance[i,0];
					        minOpIndex = i;
					        for (int k=0; k<this.numberOfClusters; k++) 
                            {
						        if (min > euclideanDistance[i,k]) 
                                {
							        min = euclideanDistance[i,k];
							        minIndex = k;
						        }
					        }
					        break;
				        }
			        }

			        // For all output nodes, compute the outputs of all the labels/cluster
			        // nodes
			        for (int l=0; l< this.numberOfOutputs; l++) 
                    {
				        for (int k=0; k< this.numberOfClusters; k++) 
                        {
					        // calculate the output of the network
					        this.synapticInputs[l,k] = 0.0;
					        
                            // calculate total synaptic inputs to each node
					        for (int i=0; i<numberOfInputs; i++) 
                            {
						        this.synapticInputs[l,k] += Math.Abs(inputdata[j, i] - this.clusters[l,k,i]);
					        }

					        // calculate the node probability & node output
					        this.nodeProbabilities[l, k] = 1.0/(1.0 + Math.Exp( - this.synapticInputs[l,k]));

					        // generate the node's threshold

                            Random random = new Random();

                            double threshold = random.NextDouble();
					        while(threshold < 0.55)
                            {
                                threshold = random.NextDouble();
					        }
                            if (this.nodeProbabilities[l, k] > threshold)
                            {
                                this.synapticOutputs[l, k] = 1.0;
                            }
                            else
                            {
                                this.synapticOutputs[l, k] = 0.0;
                            }
				        }
			        }

			        // Update the winning label/cluster node of the desired output node
			        for (int k=0; k< this.numberOfClusters; k++) 
                    {

				        // Update the winner & its neighbouring cluster nodes using REINFORCE algorithm
				        // compute the continuous reinforcement signal [0, 1]
				        double power = 0.0;
				        for (int i=0; i< this.numberOfInputs; i++) 
                        {
					        power += Math.Abs(this.clusters[minOpIndex, k, i] - this.clusters[minOpIndex, minIndex, i]);
				        }
				        
                        double reinforcement = Math.Exp( - power / 0.01);

				        // compute vector (X-W)
				        for (int i=0; i < this.numberOfInputs; i++) 
                        {
					        distVector[i] = (inputdata[j, i] - this.clusters[minOpIndex, k, i]);
				        }		

				        // update the centroids
				        for (int i=0; i< this.numberOfInputs; i++) 
                        {
					        // if Y = 0, perform learning on winner
					        if (synapticOutputs[minOpIndex, k] < 0.9)
                            {
						        this.clusters[minOpIndex, k, i] += 2.0 * LEARN_CONST * reinforcement * this.nodeProbabilities[minOpIndex, k] * distVector[i];
					        }
					        // else if Y = 1, perform unlearning on winner
					        else 
                            {
						        this.clusters[minOpIndex, k, i] += 2.0 * LEARN_CONST * reinforcement * ( this.nodeProbabilities[minOpIndex, k] - 1.0) * distVector[i];
					        }
				        }
			
                        /*
				        // the winner node is found, using Self-organizing algorithm
				        if (k == min_index) {
					        for (i=0; i<no_inputs; i++) {
						        distVector[i] = (inputdata[j][i]-clusterPtr[min_opindex][k][i]);
					        }		
					        for (i=0; i<no_inputs; i++) {
						        clusterPtr[min_opindex][k][i] += LEARN_CONST*distVector[i];
					        }
				        }
				        // end of Self-organizing algorithm
                        */

			        }
		        }
	        }

	        // calculate the corresponding widths
	        for (int k=0; k < this.numberOfOutputs; k++) 
            {
		        for (int i=0; i < this.numberOfInputs; i++) 
                {
			        // compute first & last element since they have no RHS & LHS neighbours
			        double RHScentroid = Math.Abs( this.clusters[k,0,i] - this.clusters[k, 1, i]);
			        clusterWidths[k, 0, i] = RHScentroid / WIDTH_CONSTANT;
			        double LHScentroid = Math.Abs( this.clusters[k, this.numberOfClusters - 1, i] - this.clusters[k, this.numberOfClusters - 2, i]);
			        this.clusterWidths[k, this.numberOfClusters - 1, i] = LHScentroid / WIDTH_CONSTANT;
		
			        // compute the others
			        for (int j=1; j< this.numberOfClusters - 1; j++) 
                    {
				        LHScentroid = Math.Abs( this.clusters[k, j, i] - this.clusters[k, j-1, i]);
				        RHScentroid = Math.Abs( this.clusters[k, j, i] - this.clusters[k, j+1, i]);
				        if (LHScentroid <= RHScentroid) 
                        {
					        this.clusterWidths[k, j, i] = LHScentroid / WIDTH_CONSTANT;
				        }
				        if (RHScentroid < LHScentroid) 
                        {
					        this.clusterWidths[k, j, i] = RHScentroid / WIDTH_CONSTANT;
				        }
			        }
		        }
	        }

	        Console.WriteLine("Final Clusters: ");
	        for (int k=0; k< this.numberOfOutputs; k++) 
            {
		        Console.WriteLine("For output node {0:D}: ", k);
		        for (int j=0; j< this.numberOfClusters; j++) 
                {
			        Console.Write("For cluster {0:D}: \t", j);
                    for (int i = 0; i < this.numberOfInputs; i++)
                    {
                        Console.Write("{0:F3}\t", this.clusters[k, j, i]);
                    }
                    Console.WriteLine();
		        }
	        }
        }

        public double testOutputClassification(double [,] inputdata, double [,] outputdata, int count) 
        {
	        int correct = 0;

            int [] class_results = new int [this.numberOfOutputs];
            int [] class_total = new int [this.numberOfOutputs];
            for (int i=0; i < this.numberOfOutputs; i++) 
            {
		        class_results[i] = 0;
		        class_total[i] = 0;
	        }

            double [,] eucli_dist = new double [this.numberOfOutputs, this.numberOfClusters];
	        for (int j=0; j < count; j++) 
            {
		        for (int i=0; i < this.numberOfOutputs; i++) 
                {
			        for (int k=0; k < this.numberOfClusters; k++) 
                    {
				        eucli_dist[i, k] = 0.0;
				        for (int l=0; l < this.numberOfInputs; l++) 
                        {
					        eucli_dist[i, k] += (inputdata[j, l] - this.clusters[i, k, l]) * (inputdata[j, l] - this.clusters[i, k, l]);
				        }
			        }
		        }

		        // Find the cluster with the smallest euclidean distance(i.e the winner)
		        int min_opindex = 0;
		        int min_index = 0;
		        double min = eucli_dist[0, 0];
		        for (int i=0; i < this.numberOfOutputs; i++) 
                {
			        for (int k=0; k < this.numberOfClusters; k++) 
                    {
				        if (min > eucli_dist[i, k]) 
                        {
					        min = eucli_dist[i, k];
					        min_index = k;
					        min_opindex = i;
				        }
			        }
		        }

		        for (int i=0; i < this.numberOfOutputs; i++) 
                {
                    if (outputdata[j, i] == 1.0)
                    {
                        class_total[i]++;
                    }
		        }

		        if ( outputdata[j, min_opindex] == 1.0) 
                {
			        class_results[min_opindex]++;
			        correct++;
		        }
	        }

	        for (int i=0; i< this.numberOfOutputs; i++) 
            {
                Console.WriteLine("class[{0:D}]: {1:F3} percent ({2:D} of {3:D})", i, 100.0 * ((double) class_results[i] / (double)class_total[i]), class_results[i], class_total[i]);
	        }

            Console.WriteLine("classification results: {0:F3} percent\n", 100.0 * ((double)correct / (double)count));

	        return 100.0*((float)correct/(float)count);
        }

        public void averageCentroidWidth() 
        {
	        this.centroids = new double [this.numberOfClusters, this.numberOfInputs];
            this.widths = new double [this.numberOfClusters, this.numberOfInputs];

	        // Total up the centroids
	        for (int j=0; j < this.numberOfInputs; j++) 
            {
		        for (int k=0; k< this.numberOfClusters; k++) 
                {
			        this.centroids[k, j] = 0.0;
			        for (int i=0; i < this.numberOfOutputs; i++) 
                    {
				        this.centroids[k, j] += this.clusters[i, k, j];
			        }
		        }
	        }
	        for (int k=0; k < this.numberOfClusters; k++) 
            {
		        for (int j=0; j < this.numberOfInputs; j++) 
                {
			        this.centroids[k, j] = this.centroids[k, j]/(double)this.numberOfOutputs ;
		        }
	        }

	        // calculate the corresponding widths
	        for (int i=0; i< this.numberOfInputs; i++) 
            {
		        // compute first & last element since they have no RHS & LHS neighbours
		        double RHScentroid = Math.Abs(this.centroids[0, i] - this.centroids[1, i]);
		        this.widths[0, i] = RHScentroid / WIDTH_CONSTANT;
		        double LHScentroid = Math.Abs(this.centroids[this.numberOfClusters-1, i] - this.centroids[this.numberOfClusters - 2, i]);
		        this.widths[this.numberOfClusters - 1, i] = LHScentroid / WIDTH_CONSTANT;
		
		        // compute the others
		        for (int j=1; j < this.numberOfClusters - 1; j++) 
                {
			        LHScentroid = Math.Abs(this.centroids[j, i] - this.centroids[j-1, i]);
			        RHScentroid = Math.Abs(this.centroids[j, i] - this.centroids[j+1, i]);
			        if (LHScentroid <= RHScentroid) 
                    {
				        this.widths[j, i] = LHScentroid / WIDTH_CONSTANT;
			        }
			        if (RHScentroid < LHScentroid) 
                    {
				        this.widths[j, i] = RHScentroid / WIDTH_CONSTANT;
			        }
		        }
	        }
	
	        Console.WriteLine("Final Clusters (Average): ");
	        for (int i=0; i < this.numberOfInputs; i++) 
            {
                for (int j = 0; j < this.numberOfClusters; j++)
                {
                    Console.Write("{0:F3}\t", this.centroids[j, i]);
                }

                Console.WriteLine() ;
	        }
        }


    }
}
