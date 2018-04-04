import java.util.Random;

public class Producer implements Runnable {
	private Random rand;
	private int id;
	private Buffer buffer;

	public Producer(Buffer buffer,int id) {
		super();
		this.rand = new Random();
		this.id=id;
		this.buffer=buffer;
	}

	@Override
	public void run() {

		while (true) {
			try {
				Thread.sleep(500);
			} catch (InterruptedException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
			int value = rand.nextInt(100);
			buffer.put(value);
			
			
		}
	}

}

