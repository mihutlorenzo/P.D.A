import java.util.ArrayList;
import java.util.concurrent.Semaphore;
import java.util.concurrent.locks.ReentrantLock;

public class Buffer {
	private final int BUFFERLENGTH = 10;
	private int[] buffer;
	private Semaphore semFree;
	private Semaphore semFull;
	private int bufferTail, bufferHead;
	private ReentrantLock consumerLock;
	private ReentrantLock producerLock;

	public Buffer() {
		super();
		buffer = new int[BUFFERLENGTH];
		semFree = new Semaphore(BUFFERLENGTH);
		semFull = new Semaphore(0);
		bufferTail = 0;
		bufferHead = 0;
		consumerLock = new ReentrantLock();
		producerLock = new ReentrantLock();
	}

	public void put(int value) {
		try {
			semFree.acquire();
			producerLock.lock();
			buffer[bufferTail % buffer.length] = value;
			System.out.println(
					"Producer put in buffer element with value " + value + " and number in queue " + bufferTail % buffer.length);
			bufferTail++;
			producerLock.unlock();
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} finally {
			semFull.release();

		}

	}

	public int get() {
		int value = 0;
		try {
			semFull.acquire();
			consumerLock.lock();
			value = buffer[bufferHead % buffer.length];
			System.out.println(
					"Consumer get element from buffer with value " + value + " and number in queue " + bufferHead % buffer.length);
			bufferHead++;
			consumerLock.unlock();
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} finally {
			semFree.release();
		}
		return value;
	}

}
